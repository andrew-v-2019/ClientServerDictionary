using System;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        private static TcpClient CreateTcpClient(string inputString)
        {
            var ipAndPort = inputString.GetInputStringItem(0);
            var ip = ipAndPort.Split(':')[0];
            var port = Int32.Parse(ipAndPort.Split(':')[1]);
            var client = new TcpClient(ip, port);
            return client;
        }

        public static void Main(string[] args)
        {
            TcpClient client = null;
            try
            {
                var inputString = string.Join(" ", args);
                var validator = new InputStringValidator();
                var validationRes = validator.Validate(inputString);
                if (!validationRes.IsValid)
                {
                    throw new Exception(validationRes.Errors[0].ErrorMessage);
                }
                client = CreateTcpClient(inputString);
                var stream = client.GetStream();
                var requestToServer = Encoding.Unicode.GetBytes(inputString);
                stream.Write(requestToServer, 0, requestToServer.Length);

                var responseFromServer = new byte[64];
                var builder = new StringBuilder();
                var bytes = 0;
                do
                {
                    bytes = stream.Read(responseFromServer, 0, responseFromServer.Length);
                    builder.Append(Encoding.Unicode.GetString(responseFromServer, 0, bytes));
                }
                while (stream.DataAvailable);

                var messageFromServer = builder.ToString();
                Console.WriteLine(messageFromServer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
