using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _AsyncSocket_Server
{
    class Program
    {
        public static async Task Main(string[] args) // метод буде виконувати асинхронні задачі
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(ip, 1078);
            using Socket listener = new(
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            listener.Bind(iPEndPoint);
            listener.Listen(100);

            while (true)
            {
                var handler = await listener.AcceptAsync(); // тут чекає відповідь від клієнта (відправку повідомлення ітд ітп)
                while (true)
                {
                    // Receive message.
                    var buffer = new byte[1_024];
                    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);

                    var eom = "<|EOM|>";
                    if (response.IndexOf(eom) > -1 /* is end of message */)
                    {
                        Console.WriteLine(
                            $"Socket server received message: \"{response.Replace(eom, "")}\"");

                        var ackMessage = "<|ACK|>";
                        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                        await handler.SendAsync(echoBytes, 0);
                        Console.WriteLine(
                            $"Socket server sent acknowledgment: \"{ackMessage}\"");
                        //break;
                    }
                    // Sample output:
                    //    Socket server received message: "Hi friends 👋!"
                    //    Socket server sent acknowledgment: "<|ACK|>"
                }
            }
        }
    }
}
