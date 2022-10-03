using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPConsoleChat
{
    public class Program
    {
        static readonly object _lock = new object();
        static readonly Dictionary<int, TcpClient> list_clients = new Dictionary<int, TcpClient>();

        static void Main(string[] args)
        {
            int count = 1;
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1097);
            server.Start(); // запуск сєрвака
            while (true) // жду клієнтів
            {
                TcpClient client = server.AcceptTcpClient(); // спрацьовує тоді коли клієнт відправив повідомлення на сервер
                lock (_lock) list_clients.Add(count, client);
                Thread thread = new Thread(handle_client);
                thread.Start(count);
                count++;
            }
        }
        private static void handle_client(object count)
        {
            int id = (int)count;
            TcpClient client;
            lock (_lock) client = list_clients[id];
            while (true) // буде розсилати клієнтам повідомлення (цикл сам скоріш за все перерветься, якщо клієнт дізконектиться)
            {
                NetworkStream stream = client.GetStream(); // читаємо дані від клієнта
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);
                if (byte_count > 0) // якщо дані є
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, byte_count);
                    broadcast(data, id); // розсилаємо дані усім клаєнтам, які є в чаті
                    Console.WriteLine($"id#{id} :: " + data);
                }
            }
            // коли клієнт завершує своє життя (дізконектиться)
            lock (_lock) list_clients.Remove(id);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        private static void broadcast(string data, int id) // розіслати повідомлення всім хто є в чаті
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            lock (_lock)
            {
                foreach (TcpClient c in list_clients.Values) // перебирає усіх клієнтів які є
                {
                    if (list_clients[id] == c) continue;
                    NetworkStream stream = c.GetStream(); // отримує потік самого клієнта
                    stream.Write(buffer, 0, buffer.Length); // кидає повідомлення клієнту
                }
            }
        }
    }
}