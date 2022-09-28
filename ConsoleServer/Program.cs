using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServer
{
    class Program 
    { 
        public static void Main(string[] args)
        {
            string ipAddress = "127.0.0.1"; // local host ipAddress
            int port = 1076; // 1433 не проканає, бо він зарезервований MSSQL SERVER
            IPAddress ip = IPAddress.Parse(ipAddress); // перевіряє чи вірно введена айпішка
            IPEndPoint endPoint = new IPEndPoint(ip, port); // кінцева точка на яку будем стукать (точка сервака ну яку ми стукаєм. ТОЧКА НА ЯКІЙ ЗАПУСТИТЬСЯ СЕРВАК)

            Socket server = new Socket(ip.AddressFamily, // IPv4
                SocketType.Stream, // буде работать в потоці і двусторонньому потоці (тобто буде отримувати запити і відповідати на них)
                ProtocolType.Tcp); // tcp протокол - забезпечує подачу даних і перевіряє, що пакети точно доставленні (якщо не доставлені то повторно відправляє запрос). Utp - тіпа запроси в скайп (звук повідомлення ітд)

            // прив'язка сервера до кінцевої точки
            server.Bind(endPoint);
            server.Listen(1000); // максимальна черга сервера. ТІПА скільки людей одночасно може стукатись до сервака

            Console.WriteLine("Server start {0}", endPoint);

            while (true) // СЕРВЕР І КЛІЄНТ ОБМІНЮЮТЬСЯ БАЙТАМИ
            {
                Socket client = server.Accept(); // server.Accept() -> означає, що сервер очікує запит від клієнта
                byte[] buffer = new byte[1024]; // читать байти від клієнта буде
                int size = client.Receive(buffer)/*вертає кількість байтів які клієнт прислав*/; // читаємо масив байтів від клієнта
                string text = Encoding.UTF8.GetString(buffer); // перетворив байти в текст
                Console.WriteLine($"Client: {client.RemoteEndPoint} \n Message: {text}"); // хто дав запрос і який текст 
                byte[] clientSendData = Encoding.UTF8.GetBytes($"Лови рашист гранату {DateTime.Now}");
                client.Send(clientSendData);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }
    }
}

