// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Tilde.Pixel.Opc
{
    [Flags]
    public enum PixelStreamFormat : int
    {
        None = 0,
        Rgb = 1,
        Rgba = 2,
        RgbF = 4,
        RgbaF = 8,
        Png = 16,
        PngRgb = Png | Rgb,
        PngRgba = Png | Rgba,
        GZip = 32,
        GZipRgb = GZip | Rgb,
        GZipRgba = GZip | Rgba,
    }

    public class OpenPixelControlServer : IDisposable
    {
        private readonly ConcurrentDictionary<int, ServerSession> sessions = new ConcurrentDictionary<int, ServerSession>();

        private Task acceptTcpClientTask;

        private Bitmap bitmap;
        private ColorRgb[] colorRgb = new ColorRgb[0];
        private ColorRgba[] colorRgba = new ColorRgba[0];
        private ColorRgbaF[] colorRgbaF = new ColorRgbaF[0];
        private ColorRgbF[] colorRgbF = new ColorRgbF[0];
        private readonly MemoryStream compressionMemoryStream;
        private readonly BinaryWriter compressionWriter;
        private byte[] data = new byte[0];
        private MemoryStream memoryStream;
        private TcpListener tcpListener;

        private BinaryWriter writer;

        private readonly Stopwatch stopwatch; 

        public Uri Address { get; private set; }

        public IColorBuffer Buffer { get; }

        public bool Connected { get; private set; }

        public PixelStreamFormat Format { get; set; } = PixelStreamFormat.PngRgb;

        public int PixelStride
        {
            get
            {
                switch (Format)
                {
                    case PixelStreamFormat.Rgb:
                        return ColorRgb.SizeOf;

                    case PixelStreamFormat.Rgba:
                        return ColorRgba.SizeOf;

                    case PixelStreamFormat.RgbF:
                        return ColorRgbF.SizeOf;

                    case PixelStreamFormat.RgbaF:
                        return ColorRgbaF.SizeOf;

                    case PixelStreamFormat.PngRgb:
                        return ColorRgb.SizeOf;

                    case PixelStreamFormat.PngRgba:
                        return ColorRgba.SizeOf;

                    case PixelStreamFormat.GZipRgb:
                        return ColorRgb.SizeOf;

                    case PixelStreamFormat.GZipRgba:
                        return ColorRgba.SizeOf;

                    case PixelStreamFormat.None:
                    case PixelStreamFormat.Png:
                    case PixelStreamFormat.GZip:
                    default:
                        return 0;
                }
            }
        }
        
        public int Port { get; set; }

        public int Height => Buffer.Height;

        public int Width => Buffer.Width;

        public OpenPixelControlServer()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start(); 

            compressionMemoryStream = new MemoryStream();
            compressionWriter = new BinaryWriter(compressionMemoryStream);

            memoryStream = new MemoryStream(data);
            writer = new BinaryWriter(memoryStream);
        }

        public void Bind()
        {
            base.Bind();

            Buffer.Bind();
            TimingSource.Bind();

            TimingSource.Value.Pulse += Run;

            try
            {
                if (Connected == true)
                {
                    return;
                }

                IOscNetworkConnectionInfo connectionInfo = OscConnection.ConnectionInfo as IOscNetworkConnectionInfo;

                IPAddress adapterAddress = connectionInfo.NetworkAdapterIPAddress;
                IPAddress serviceIP = adapterAddress;

                if (adapterAddress == IPAddress.Any)
                {
                    try
                    {
                        foreach (IPAddress address in Dns.GetHostEntry(Environment.MachineName).AddressList)
                        {
                            if (address.AddressFamily != AddressFamily.InterNetwork)
                            {
                                continue;
                            }

                            serviceIP = address;

                            break;
                        }
                    }
                    catch
                    {
                        serviceIP = IPAddress.Loopback;
                    }
                }

                Address = new Uri($"net.tcp://{serviceIP}:{Port}");

                tcpListener = new TcpListener(connectionInfo.NetworkAdapterIPAddress, Port);

                tcpListener.Start();

                // use the ipaddress as in the server program
                OscConnection.Reporter.PrintEmphasized(Direction.Action, Name.OscAddress, $"Image stream server listening: {Address}");

                Connected = true;

                acceptTcpClientTask = AcceptTcpClientTask();
            }
            catch (Exception ex)
            {
                OscConnection.Reporter.PrintError(Direction.Action, Name.OscAddress, $"Error creating image stream server: {Address}");
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            try
            {
                if (Connected == true)
                {
                    Connected = false;

                    foreach (ServerSession session in sessions.Values)
                    {
                        session.Close();
                    }

                    OscConnection.Reporter.PrintEmphasized(Direction.Action, Name.OscAddress, $"Disconnected from open pixel control server: {Address}");
                }

                Connected = false;
            }
            catch (Exception ex)
            {
                OscConnection.Reporter.PrintError(Direction.Action, Name.OscAddress, $"Error disconnecting to open pixel control server: {Address}");
            }
        }

        [OscMethod]
        public override void Unbind()
        {
            base.Unbind();

            TimingSource.Value.Pulse -= Run;
            TimingSource.Unbind();
            Buffer.Unbind();

            Dispose();
        }

        private async Task AcceptTcpClientTask()
        {
            while (Connected == true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                OscConnection.Reporter.PrintDetail(Direction.Receive, Name.OscAddress, "Image stream server client");

                Task task = StartHandleConnection(tcpClient);

                // if already faulted, re-throw any error on the calling context
                if (task.IsFaulted == true)
                {
                    task.Wait();
                }
            }
        }

        private async Task EndHandleConnection(TcpClient tcpClient)
        {
            try
            {
                // continue asynchronously on another threads
                await Task.Yield();

                ServerSession session = new ServerSession(tcpClient);

                sessions[session.GetHashCode()] = session;
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private void Run(float deltaTime)
        {
            if (sessions.Count == 0)
            {
                return;
            }

            if (Enabled == false)
            {
                return;
            }

            if (Buffer.Value == null)
            {
                return;
            }

            IColorBuffer colorBuffer = Buffer.Value;

            PixelStreamFormat format = Format;
            int width = colorBuffer.CurrentWidth;
            int height = colorBuffer.CurrentHeight;

            if (bitmap == null
                || bitmap.Width != width
                || bitmap.Height != height
                || (format & PixelStreamFormat.Rgb) == PixelStreamFormat.Rgb && bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb
                || (format & PixelStreamFormat.Rgba) == PixelStreamFormat.Rgba && bitmap.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                bitmap?.Dispose();

                if ((format & PixelStreamFormat.Rgb) == PixelStreamFormat.Rgb)
                {
                    bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }
                else if ((format & PixelStreamFormat.Rgba) == PixelStreamFormat.Rgba)
                {
                    bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }
            }

            if (colorRgb.Length != Buffer.Value.BufferSize)
            {
                colorRgb = new ColorRgb[Buffer.Value.BufferSize];
                colorRgba = new ColorRgba[colorRgb.Length];
                colorRgbF = new ColorRgbF[colorRgb.Length];
                colorRgbaF = new ColorRgbaF[colorRgb.Length];
            }

            stopwatch.Restart(); 

            switch (format)
            {
                case PixelStreamFormat.None:
                    //SendBuffer(((ColorBuffer<ColorRgb>)Buffer.Value).Buffer);
                    break;

                case PixelStreamFormat.Rgb:
                    colorBuffer.CopyTo(colorRgb);
                    SendBuffer(colorRgb, format, width, height);
                    break;

                case PixelStreamFormat.Rgba:
                    colorBuffer.CopyTo(colorRgba);
                    SendBuffer(colorRgba, format, width, height);
                    break;

                case PixelStreamFormat.RgbF:
                    colorBuffer.CopyTo(colorRgbF);
                    SendBuffer(colorRgbF, format, width, height);
                    break;

                case PixelStreamFormat.RgbaF:
                    colorBuffer.CopyTo(colorRgbaF);
                    SendBuffer(colorRgbaF, format, width, height);
                    break;

                case PixelStreamFormat.PngRgb:
                    colorBuffer.CopyTo(colorRgb);
                    WriteToBitmap(colorRgb, bitmap);
                    SendPng(bitmap, format, width, height);
                    break;

                case PixelStreamFormat.PngRgba:
                    colorBuffer.CopyTo(colorRgba);
                    WriteToBitmap(colorRgba, bitmap);
                    SendPng(bitmap, format, width, height);
                    break;

                case PixelStreamFormat.GZipRgb:
                    colorBuffer.CopyTo(colorRgb);
                    SendGZip(colorRgb, format, width, height);
                    break;

                case PixelStreamFormat.GZipRgba:
                    colorBuffer.CopyTo(colorRgba);
                    SendGZip(colorRgba, format, width, height);
                    break;

                default:
                    break;
            }            
        }

        private void Send(byte[] data, int length)
        {
            if (Connected == false)
            {
                return;
            }

            foreach (KeyValuePair<int, ServerSession> sessionPair in sessions)
            {
                if (sessionPair.Value.Connected == false)
                {
                    ServerSession temp;
                    sessions.TryRemove(sessionPair.Key, out temp);

                    continue;
                }

                sessionPair.Value.Send(data, length);
            }
        }

        private void SendBuffer<T>(T[] buffer, PixelStreamFormat format, int width, int height)
        {
            int headerSize = 16;

            int dataSize = buffer.Length * Marshal.SizeOf(typeof(T));

            if (data.Length != dataSize + headerSize)
            {
                data = new byte[dataSize + headerSize];

                memoryStream.Dispose();

                memoryStream = new MemoryStream(data);
                writer = new BinaryWriter(memoryStream);
            }

            memoryStream.Position = 0;

            writer.Write((int)format);
            writer.Write(width);
            writer.Write(height);
            writer.Write(dataSize);

            int index = headerSize;

            ColorUtils.CopyToByteArray(buffer, buffer.Length, data, ref index);

            memoryStream.Position = index;

            //Console.WriteLine($"Stream size: {index} ({format} {width} {height} [{dataSize + headerSize}] {stopwatch.ElapsedMilliseconds}ms)");

            Send(data, dataSize + headerSize);
        }

        private void SendGZip<T>(T[] colors, PixelStreamFormat format, int width, int height)
        {
            compressionMemoryStream.Position = 0;

            compressionWriter.Write((int)format);
            compressionWriter.Write(width);
            compressionWriter.Write(height);

            int sizePosition = (int)compressionMemoryStream.Position;
            compressionWriter.Write((int)0);

            int begin = (int)compressionMemoryStream.Position;
            using (GZipStream gzipSream = new GZipStream(compressionMemoryStream, CompressionLevel.Optimal, true))
            {
                WriteToMemoryStream(colors);

                gzipSream.Write(data, 0, data.Length);
            }

            int finalSize = (int)compressionMemoryStream.Position;

            compressionMemoryStream.Position = sizePosition;
            compressionWriter.Write((int)(finalSize - begin));
            compressionMemoryStream.Position = finalSize;

            //Console.WriteLine($"GZIP stream size: {finalSize} ({format} {width} {height} [{finalSize - begin}]) {stopwatch.ElapsedMilliseconds}ms");

            Send(compressionMemoryStream.GetBuffer(), finalSize);
        }

        private void SendPng(Bitmap bitmap, PixelStreamFormat format, int width, int height)
        {
            compressionMemoryStream.Position = 0;

            compressionWriter.Write((int)format);
            compressionWriter.Write(width);
            compressionWriter.Write(height);

            int sizePosition = (int)compressionMemoryStream.Position;
            compressionWriter.Write((int)0);

            int begin = (int)compressionMemoryStream.Position;

            bitmap.Save(compressionMemoryStream, ImageFormat.Png);

            int finalSize = (int)compressionMemoryStream.Position;

            compressionMemoryStream.Position = sizePosition;
            compressionWriter.Write((int)(finalSize - begin));
            compressionMemoryStream.Position = finalSize;

            //Console.WriteLine($"PNG stream size: {finalSize} ({format} {width} {height} [{finalSize - begin}]) {stopwatch.ElapsedMilliseconds}ms");

            Send(compressionMemoryStream.GetBuffer(), finalSize);
        }

        private async Task StartHandleConnection(TcpClient tcpClient)
        {
            // start the new connection task
            var connectionTask = EndHandleConnection(tcpClient);

            // catch all errors of HandleConnectionAsync
            try
            {
                await connectionTask;
                // we may be on another thread after "await"
            }
            catch (Exception ex)
            {
                OscConnection.Reporter.PrintException(ex, "Exception while processing REST request");
            }
        }

        private void WriteToBitmap(ColorRgba[] colorRgba, Bitmap bitmap)
        {
            //Create a BitmapData and Lock all pixels to be written
            BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            ColorUtils.CopyToPointer(colorRgba, colorRgba.Length, bmpData.Scan0);

            //Unlock the pixels
            bitmap.UnlockBits(bmpData);
        }

        private void WriteToBitmap(ColorRgb[] colorRgb, Bitmap bitmap)
        {
            //Create a BitmapData and Lock all pixels to be written
            BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Copy the data from the byte array into BitmapData.Scan0
            ColorUtils.CopyToPointer(colorRgb, colorRgb.Length, bmpData.Scan0);

            //Unlock the pixels
            bitmap.UnlockBits(bmpData);
        }

        private void WriteToMemoryStream<T>(T[] buffer)
        {
            int dataSize = buffer.Length * Marshal.SizeOf(typeof(T));

            if (data.Length != dataSize)
            {
                data = new byte[dataSize];

                memoryStream.Dispose();

                memoryStream = new MemoryStream(data);
                writer = new BinaryWriter(memoryStream);
            }

            memoryStream.Position = 0;

            int index = 0;

            ColorUtils.CopyToByteArray(buffer, buffer.Length, data, ref index);

            memoryStream.Position = index;
        }

        private class ServerSession
        {
            private TcpClient client;
            private Task sendTask;
            private NetworkStream stream;
            public bool Connected { get; private set; }

            public ServerSession(TcpClient client)
            {
                this.client = client;
                stream = client.GetStream();

                Connected = true;
            }

            public void Close()
            {
                Connected = false;

                client?.Close();
                client = null;
            }

            public void Send(byte[] data, int length)
            {
                if (Connected == false)
                {
                    return;
                }

                if (sendTask != null)
                {
                    switch (sendTask.Status)
                    {
                        case TaskStatus.Canceled:
                        case TaskStatus.Faulted:
                            Connected = false;
                            return;

                        case TaskStatus.RanToCompletion:
                            break;

                        default:
                            return;
                    }
                }

                try
                {
                    sendTask = stream.WriteAsync(data, 0, length);
                }
                catch
                {
                    Close();
                }
            }
        }
    }
}