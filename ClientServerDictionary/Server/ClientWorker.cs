using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientWorker
    {
        private TcpClient _client;
        private DistionaryService _service;

        public ClientWorker(TcpClient client)
        {
            _service = new DistionaryService();
            _client = client;
        }

        private List<string> DoOperation(string requestFromClient)
        {
            var operationResult = new List<string>();
            var command = requestFromClient.Split(' ')[1].Trim().ToLower();
            var term = requestFromClient.Split(' ')[2].Trim().ToLower();
            var args = requestFromClient.Split(' ').Skip(3).ToList();
            if (command.Equals("add"))
            {
                _service.Add(term, args);
                operationResult.Add("Added");
            }
            else
            {
                if (command.Equals("get"))
                {
                    var r = _service.Get(term);
                    operationResult = r.ToList();
                }
                else
                {
                    _service.Delete(term, args);
                    operationResult.Add("Removed");
                }
            }
            return operationResult;
        }

        public void Process()
        {
            var stream = _client.GetStream();
            try
            {
                var data = new byte[64];
                var builder = new StringBuilder();
                var bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                var requestFromClient = builder.ToString();

                var responseToClient = DoOperation(requestFromClient);
                Console.WriteLine(string.Format("Response to request '{0}' is {1}", requestFromClient, string.Join(" ", responseToClient)));
                foreach (var item in responseToClient)
                {
                    data = Encoding.Unicode.GetBytes(string.Format("{0}\n\r", item));
                    stream.Write(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                var data = Encoding.Unicode.GetBytes(ex.Message + "; " + ex.InnerException);
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (_client != null) _client.Close();
            }
        }
    }
}
