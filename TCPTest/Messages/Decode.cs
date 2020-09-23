using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace TCPTest.Messages
{
    class Decode
    {
        private byte[] _buffer;
        private int _bufferLength;
        private Message _message;
        ConcurrentQueue<ScreeMessage> _screeMessages;

        public Decode(byte[] buffer, int bufferLength, ConcurrentQueue<ScreeMessage> screeMessages)
        {
            _buffer = buffer;
            _bufferLength = bufferLength;
            _message = new Message();
            _screeMessages = screeMessages;

            //DecodeBuffer();
        }
        
        char Get8(int place)
        {
            return (char)_buffer[place];
        }
        int Get16AsInt(int place)
        {
            byte[] b = new byte[2];
            b[0] = _buffer[place];
            b[1] = _buffer[place + 1];
            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);

            return BitConverter.ToInt16(b);

        }

        int Get32AsInt(int place)
        {
            byte[] b = new byte[4];
            b[0] = _buffer[place];
            b[1] = _buffer[place + 1];
            b[2] = _buffer[place + 2];
            b[3] = _buffer[place + 3];

            if (BitConverter.IsLittleEndian)
                Array.Reverse(b);

            return BitConverter.ToInt32(b);

        }

        double GetAsDouble(int place)
        {
            byte[] b = new byte[8];
            b[0] = _buffer[place];
            b[1] = _buffer[place + 1];
            b[2] = _buffer[place + 2];
            b[3] = _buffer[place + 3];
            b[4] = _buffer[place + 4];
            b[5] = _buffer[place + 5];
            b[6] = _buffer[place + 6];
            b[7] = _buffer[place + 7];
            return BitConverter.ToDouble(b);

        }

        string GetString(int place, int length)
        {
            byte[] b = new byte[length];
            for (int n = 0; n < length; n++)
            {
                b[n] = _buffer[place + n];
            }

            return BitConverter.ToString(b);

        }

        byte[] GetNonCString(int place, int length)
        {
            byte[] b = new byte[length];
            for (int n = 0; n < length; n++)
            {
                b[n] = _buffer[place + n];
            }

            return b;
        }

        public Message DecodeBuffer()
        {
            int i = 0;
            int ver;
            int len;
            ver = (int)Get8(i);
            i++;
           
            _message.Type = (int)Get8(i);
            i++;
            
            var text = $"Message type recieved: {_message.Type}";
            _screeMessages.Enqueue(new ScreeMessage
            {
                xPos = 1,
                yPos = 5,
                message = text,
                align = ScreeMessage.Align.Left,
                wAlign = ScreeMessage.WAlign.Bottom,
                maxLength = 30
            });

            len = Get16AsInt(i);
            i =  i +2;
            
            _message.Sender = Get32AsInt(i);
            i = i + 4;
            
            switch (_message.Type)
            {
                case (int)Message.MessageTypes.MSG_UPDATE_LABEL:
                    _message.MsgUpdateLabel.Expose = Encoding.UTF8.ReadCString(GetNonCString(i, 64)); //GetString(i, 64);
                    i = i + 64;
                    
                    _message.MsgUpdateLabel.Text = Encoding.UTF8.ReadCString(GetNonCString(i, 64));
                    i = i + 64;
                    
                    _message.MsgUpdateLabel.Text2 = Encoding.UTF8.ReadCString(GetNonCString(i, 64));
                    i = i + 64;
                    
                    _message.MsgUpdateLabel.Text3 = Encoding.UTF8.ReadCString(GetNonCString(i, 16));
                    i = i + 16;

                    _message.MsgUpdateLabel.Y = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.Y = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.W = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.H = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.FgR = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.FgG = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.FgB = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.BgR = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.BgG = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.BgB = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.Size = GetAsDouble(i);
                    i = i + 8;

                    _message.MsgUpdateLabel.LabelNum = (int)Get32AsInt(i);
                    i=i+4;
                    
                    _message.MsgUpdateLabel.Xalign = (int)Get32AsInt(i);
                    i=i+4;
                    
                    break;
                default:
                    break;
            }

            return _message;
        }

    }
}
