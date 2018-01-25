using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Server
    {
        private const int _port = 8888;
        private const string _address = "127.0.0.1";
        private static TcpListener _listener;
        static void Main(string[] args)
        {
            try
            {
                _listener = new TcpListener(IPAddress.Parse(_address), _port);
                _listener.Start();
                Console.WriteLine(string.Format("Server has started at {0}:{1}...", _address, _port));
                while (true)
                {
                    var client = _listener.AcceptTcpClient();
                    var clientWork = new ClientWorker(client);
                    // Create new tread for user
                    var clientThread = new Thread(new ThreadStart(clientWork.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                if (_listener != null) _listener.Stop();
            }
        }
    }
}