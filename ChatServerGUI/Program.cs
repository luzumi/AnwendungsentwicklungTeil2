using System;
using System.Collections.Generic;

namespace ChatServerGUI
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hallo, ich bin ein Chat server Verwaltungsprogramm");
            Console.WriteLine("Sie können jetzt kommandos eingeben");
            Console.WriteLine("   /start            startet den Server und Reader und wartet auf Verbindungen");
            Console.WriteLine("   /stop             trennt alle verbindungen und fährt den server herunter");
            Console.WriteLine("   /status           gibt den Verbindungsstatus zurück");
            Console.WriteLine("   /send <Message>   sendet eine Nachricht an den Client");
            Console.WriteLine("   /shutdown         Ende Gelände");

            ChatServerLogic.ChatServer chatServer = new((x) => Console.WriteLine("Empfangen: {0}", x));

#if DEBUG
            chatServer.StartListenerAsync();
#endif

            string input;
            string command;
            while (true)
            {
                input = Console.ReadLine();
                int spaceID = input.IndexOf(" ");
                command = input.Substring(0, (spaceID > 0 ? spaceID : input.Length));

                switch (command)
                {
                    case "/start":
                        chatServer.StartListenerAsync();
                        break;
                    case "/stop":
                        chatServer.StopListener();
                        break;
                    case "/status":
                        List<string> status = chatServer.GetConnectionStatus();
                        foreach (var stat in status) Console.WriteLine(stat);
                        break;
                    case "/send":
                        chatServer.SendMessage(input[6..]);
                        break;
                    case "/shutdown":
                        Console.WriteLine("ENDE");
                        chatServer.StopListener();
                        chatServer.Kick();
                        return;
                    default:
                        Console.WriteLine("unbekanntes Kommando : {0}", command);
                        break;
                }
            }
        }
    }
}
