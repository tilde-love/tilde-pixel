// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Tilde.Pixel.Led;
using System;
using System.IO;
using System.Net.Sockets;

namespace Tilde.Pixel.Opc
{
    public class OpenPixelControlClient : IDisposable
    {
        private byte[] data = new byte[4];
        private ColorRgb[] colorBuffer = new ColorRgb[0]; 

        private Stream stream;
        private TcpClient tcpClient;

        public Uri Address { get; set; }

        public int Channel { get; set; }

        public bool Connected { get; private set; }

        public float Gain { get; set; } = 1f;

        public bool GrbOrder { get; set; }

        public LedMapping Mapping { get; private set; }

        public void Bind()
        {
//            base.Bind();
//
//            Mapping.Bind();
//            TimingSource.Bind();
//
//            TimingSource.Value.Pulse += Run;
//
//            try
//            {
//                if (Connected == true)
//                {
//                    return;
//                }
//
//                tcpClient = new TcpClient { NoDelay = true };
//                tcpClient.Connect(Address.Host, Address.Port);
//
//                // use the ipaddress as in the server program
//                OscConnection.Reporter.PrintEmphasized(Direction.Action, Name.OscAddress, $"Connected to open pixel control server: {Address}");
//
//                stream = tcpClient.GetStream();
//
//                Connected = true;
//            }
//            catch (Exception ex)
//            {
//                OscConnection.Reporter.PrintError(Direction.Action, Name.OscAddress, $"Error connecting to open pixel control server: {Address}");
//            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            try
            {
                if (Connected == true)
                {
                    tcpClient.Close();

                    stream.Dispose();

//                    OscConnection.Reporter.PrintEmphasized(Direction.Action, Name.OscAddress, $"Disconnected from open pixel control server: {Address}");
                }

                Connected = false;
            }
            catch (Exception ex)
            {
//                OscConnection.Reporter.PrintError(Direction.Action, Name.OscAddress, $"Error disconnecting to open pixel control server: {Address}");
            }
        }

        public void Send(byte channel, byte command, byte[] data)
        {
            if (Connected == false)
            {
                return;
            }

            data[0] = channel;
            data[1] = command;
            data[2] = ((byte)((ushort)data.Length >> 8));
            data[3] = ((byte)(data.Length & 0xFF));

            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        public void Unbind()
        {
//            base.Unbind();
//
//            TimingSource.Value.Pulse -= Run;
//            TimingSource.Unbind();
//            Mapping.Unbind();
//
//            Dispose();
        }

        private void Run(float deltaTime)
        {
            if (data.Length != Mapping.Pixels.Length * 3 + 4)
            {
                data = new byte[Mapping.Pixels.Length * 3 + 4];
                colorBuffer = new ColorRgb[Mapping.Pixels.Length]; 
            }

            BufferRgbF buffer = Mapping.Buffer;

            int index = 0;
            foreach (Pixel pixel in Mapping.Pixels)
            {
                ColorRgb color = ((ColorRgbF)buffer.GetPixelValue(pixel) * Gain);

                if (GrbOrder == false)
                {
                    color.ReverseRB(); 
                }

                colorBuffer[index++] = color;
            }

            index = 4;

            ColorUtils.CopyToByteArray(colorBuffer, colorBuffer.Length, data, ref index); 

            Send((byte)Channel, 0, data);
        }
    }
}