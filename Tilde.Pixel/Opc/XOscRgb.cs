// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Net;
using Tilde.Pixel.Led;

namespace Tilde.Pixel.Opc
{
//    public class XOscRgb : IDisposable
//    {
//        private byte[] data = new byte[4];
//        private ColorRgb[] colorBuffer = new ColorRgb[0];
//
//        private OscSender oscSender;
//
//        public Uri Address { get; set; }
//
//        public int Channel { get; set; }
//
//        public bool Connected { get; private set; }
//
//        public float Gain { get; set; } = 1f;
//
//        public bool GrbOrder { get; set; }
//
//        public LedMapping Mapping { get; private set; }
//
//        public XOscRgb()
//        {
//            Mapping = new ObjectBinding<LedMapping>(this);
//            TimingSource = new ObjectBinding<IPulse>(this);
//        }
//
//        public override void Bind()
//        {
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
//                IPAddress sendAddress = Dns.GetHostAddresses(Address.Host).FirstOrDefault();
//
//                oscSender = new OscSender(sendAddress, 0, Address.Port); 
//
//                oscSender.Connect();
//
//                // use the ipaddress as in the server program
//                OscConnection.Reporter.PrintEmphasized(Direction.Action, Name.OscAddress, $"Connected to X-OSC: {Address}");
//
//                Connected = true;
//            }
//            catch (Exception ex)
//            {
//                OscConnection.Reporter.PrintError(Direction.Action, Name.OscAddress, $"Error connecting to X-OSC: {Address}");
//            }
//        }
//
//        /// <inheritdoc />
//        public void Dispose()
//        {
//            try
//            {
//                if (Connected == true)
//                {
//                    oscSender.Close();
//
//                    OscConnection.Reporter.PrintEmphasized(Direction.Action, Name.OscAddress, $"Disconnected from X-OSC: {Address}");
//                }
//
//                Connected = false;
//            }
//            catch (Exception ex)
//            {
//                OscConnection.Reporter.PrintError(Direction.Action, Name.OscAddress, $"Error disconnecting from X-OSC: {Address}");
//            }
//        }
//
//        public void Send(int channel, byte[] data)
//        {
//            if (Connected == false)
//            {
//                return;
//            }
//
//            oscSender.Send(new OscMessage("/outputs/rgb/" + channel, data));
//        }
//
//        public void Unbind()
//        {
//            base.Unbind();
//
//            TimingSource.Value.Pulse -= Run;
//            TimingSource.Unbind();
//            Mapping.Unbind();
//
//            Dispose();
//        }
//
//        private void Run(float deltaTime)
//        {
//            if (data.Length != Mapping.Value.Pixels.Length * 3)
//            {
//                data = new byte[Mapping.Value.Pixels.Length * 3];
//                colorBuffer = new ColorRgb[Mapping.Value.Pixels.Length];
//            }
//
//            BufferRgbF buffer = Mapping.Value.Buffer.Value;
//
//            int index = 0;
//            foreach (Pixel pixel in Mapping.Value.Pixels)
//            {
//                ColorRgb color = ((ColorRgbF)buffer.GetPixelValue(pixel) * Gain);
//
//                if (GrbOrder == false)
//                {
//                    color.ReverseRB();
//                }
//
//                colorBuffer[index++] = color;
//            }
//
//            index = 0;
//
//            ColorUtils.CopyToByteArray(colorBuffer, colorBuffer.Length, data, ref index);
//
//            Send((byte)Channel, data);
//        }
//    }
}
