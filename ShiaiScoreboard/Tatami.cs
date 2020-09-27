using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ShiaiScoreboard
{
    class Tatami
    {
        static private ConcurrentQueue<string> _messageQueu;
        static private ConcurrentQueue<string> _logQueu;
        private int _tatami;
        private bool _running;
        private string _ipAddress;
        private int _port;
        private int _timeOut;
        private int _retries; //-1 = Infinity
        private int _tries;
        private TimerClient _client;

        public Tatami(
            int tatami,
             ConcurrentQueue<string> messageQueu,
             ConcurrentQueue<string> logQueu)
        {
            _messageQueu = messageQueu;
            _logQueu = logQueu;
            _tatami = tatami;
            _running = false;
            _ipAddress = "0.0.0.0";
            _port = 2312;
            _timeOut = 6000;
            _retries = -1;
            _tries = 0;

            SendMessage($"Tatami {tatami} initiated with IP Address {_ipAddress} and port {_port}.");

        }

        public void Connect(string ipAddress, int port = 2312)
        {
            _ipAddress = ipAddress;
            _port = port;

            SendMessage($"Connecting tatami {_tatami} to {_ipAddress}:{_port}.");
            _client = new TimerClient(_ipAddress, _port, _messageQueu, _logQueu, _tatami);
            _client.ConnectAsync();

        }
        public void Disconnect()
        {
            SendMessage($"Disconnecting tatami {_tatami} from {_ipAddress}:{_port}.");
            _client.DisconnectAsync();
        }
        
        private void SendMessage(string text)
        {
            _logQueu.Enqueue(text);
        }
    }
}
