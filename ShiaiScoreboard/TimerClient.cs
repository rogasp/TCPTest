using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Net.Sockets;
using System.Text;
using TcpClient = NetCoreServer.TcpClient;

namespace ShiaiScoreboard
{
    class TimerClient : TcpClient
    {
        static private ConcurrentQueue<string> _messageQueu;
        static private ConcurrentQueue<string> _logQueu;
        static private int _tatami;
        static private bool _running;

        public TimerClient(string address,
                           int port,
                           ConcurrentQueue<string> messageQueu,
                           ConcurrentQueue<string> logQueu,
                           int tatami):base(address, port)
        {
            _messageQueu = messageQueu;
            _logQueu = logQueu;
            _tatami = tatami;
           
        }

        protected override void OnConnected()
        {
            var text = $"Connected to tatami {_tatami} with session id {Id}";
            SendMessage(text);
        }

        protected override void OnDisconnected()
        {
            var text = $"Disconnected from tatami {_tatami} with session id {Id}";
            SendMessage(text);
        }

        protected override void OnError(SocketError error)
        {
            var text = $"TCP client caught an error with code {error} for tatami {_tatami}";
            SendMessage(text);
        }

        private void SendMessage(string text)
        {
            _logQueu.Enqueue(text);
        }
    }
}
