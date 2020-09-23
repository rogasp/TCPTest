using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TCPTest.Messages;
using TcpClient = NetCoreServer.TcpClient;

namespace TCPTest
{
    class TimerClient : TcpClient
    {
        private bool _stop;
        private ConcurrentQueue<Message> _queue;
        private ConcurrentQueue<ScreeMessage> _screeMessages;

        private Message _message;
        private Decode _decoder;

        public TimerClient(
            string address,
            int port,
            ConcurrentQueue<Message> queue,
            ConcurrentQueue<ScreeMessage> screeMessages) : base(address, port)
        {
            _queue = queue;
            _screeMessages = screeMessages;
        }

        public void DisconnectAndStop()
        {
            _stop = true;
            DisconnectAsync();
            while (IsConnected)
                Thread.Yield();
        }

        protected override void OnConnected()
        {
            Console.SetCursorPosition(1, 14);
            Console.Write($"Connected to timer with session id {Id}");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string msg = "";
            int n;
            int ri = 0;
            bool escape = false;
            var COMM_ESCAPE = 0xff;
            var COMM_FF = 0xfe;
            var COMM_BEGIN = 0xfd;
            var COMM_END = 0xfc;
            byte[] p = new byte[512];

            n = buffer.Length;

            for (int i = 0; i < n; i++)
            {
                char c = (char)buffer[i];

                if (c == COMM_ESCAPE)
                {
                    escape = true;
                }else if (escape)
                {
                    if(c == COMM_FF)
                    {
                       
                        if (ri < p.Length)
                        {
                            p[ri] = (byte)COMM_ESCAPE;
                            ri++;

                        }
                        
                    }
                    else if(c == COMM_BEGIN)
                    {
                        ri = 0;
                    }
                    else if( c == COMM_END)
                    {
                        _decoder = new Decode(p, ri, _screeMessages);
                        _message = _decoder.DecodeBuffer();
                    }
                    escape = false;
                }
               
                else if(ri < p.Length)
                {
                    p[ri] = (byte)c;
                    ri++;
                }
            }
            
            _queue.Enqueue(_message);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }
    }
}
