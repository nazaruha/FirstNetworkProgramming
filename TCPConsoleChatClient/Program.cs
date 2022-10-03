using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConsoleChatClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 1097;
            TcpClient tcpClient = new TcpClient();

            tcpClient.Connect(ip, port);
            Console.WriteLine("Client connect server");
            NetworkStream ns = tcpClient.GetStream();
            Thread thread = new Thread(ReceiveData);
            thread.Start(tcpClient);

            string msg;
            while (!String.IsNullOrEmpty(msg = Console.ReadLine())) // якщо клієнт щось ввів
            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                ns.Write(buffer, 0, buffer.Length);
            }
            tcpClient.Client.Shutdown(SocketShutdown.Both);
            thread.Join();
            ns.Close();
            tcpClient.Close();
            Console.WriteLine("Client disconnected from server");
            Console.ReadKey();
            
        }
        private static void ReceiveData(object o)
        {
            TcpClient client = (TcpClient)o;  
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[1024];
            int byte_count;
            while ((byte_count = ns.Read(receivedBytes, 0, 1024)) > 0)
            {
                Console.WriteLine(Encoding.UTF8.GetString(receivedBytes, 0, 1024));
            }
            
        }

    }
}