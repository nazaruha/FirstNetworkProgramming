using MailKit.Net.Imap;

namespace IMAP_POP3
{
    class Program
    {
       public static void Main(string[] args)
        {
            string username = "super.novakvova@ukr.net";
            string password = "cnNfnQDgjVmmEttB";
            /*
             * Сервер для віхдних (IMAP): imap.ukr.net.
                Порт - 993.
                З'єднання - захищене SSL.
                Безпечна перевірка пароля (SPA) вимкненя.
            */

            using (var client = new ImapClient())
            {
                client.Connect("imap.ukr.net", 993, true);

                client.Authenticate("super.novakvova@ukr.net", "cnNfnQDgjVmmEtt8");

                var inbox = client.Inbox;
                inbox.Open(MailKit.FolderAccess.ReadOnly);

                Console.WriteLine($"Total messages: {inbox.Count}");
                Console.WriteLine($"Recent messages: {inbox.Recent}");

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"HTML body: {message.HtmlBody}");
                }

                client.Disconnect(true);
            }
        }

    }
}
