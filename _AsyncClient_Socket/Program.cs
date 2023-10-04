using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _AsyncClient_Socket
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(ip, 1078);
            using Socket client = new(
                iPEndPoint.AddressFamily,
                SocketType.Stream,
                ProtocolType.Tcp);

            await client.ConnectAsync(iPEndPoint);
            while (true)
            {
                // Send message.
                //var message = "Hi friends 👋!<|EOM|>";
                Console.Write("\nYour Message: ");
                string message = Console.ReadLine(); message += "<|EOM|>";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await client.SendAsync(messageBytes, SocketFlags.None);
                Console.WriteLine($"Socket client sent message: \"{message}\"");

                // Receive ack.
                var buffer = new byte[1_024];
                var received = await client.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                if (response == "<|ACK|>")
                {
                    Console.WriteLine(
                        $"Socket client received acknowledgment: \"{response}\"");
                    //break;
                }
                else { break; }
                // Sample output:
                //     Socket client sent message: "Hi friends 👋!<|EOM|>"
                //     Socket client received acknowledgment: "<|ACK|>"
            }

            client.Shutdown(SocketShutdown.Both);
        }
    }
}
