using System.Net;
using System.Net.Sockets;
using System.Text;

string ipAddressServer = "127.0.0.1";
int portServer = 1076;

IPAddress ipAddress = IPAddress.Parse(ipAddressServer);
IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, portServer);

Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

client.Connect(ipEndPoint); // підключаємо(конектимо) клієнта до сервака

Console.WriteLine("Client connect to server {0}", ipEndPoint);
Console.WriteLine("Input message text");
string message = Console.ReadLine();

byte[] bytes = Encoding.UTF8.GetBytes(message);
client.Send(bytes);
byte[] serverResponse = new byte[1024]; // байти які пришле сервак назад клієнту
client.Receive(serverResponse);
string serverString = Encoding.UTF8.GetString(serverResponse);
Console.WriteLine("Server responce \"{0}\"", serverString);
client.Shutdown(SocketShutdown.Both);
client.Close();
