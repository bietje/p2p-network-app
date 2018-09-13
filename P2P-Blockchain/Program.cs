using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using P2P_Blockchain.Model;

namespace P2P_Blockchain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
			Server s;

            s = new Server();
            s.Start();
            handler = new ConsoleEventDelegate(ConsoleEventCallback);

            Console.WriteLine("Please enter the IP of a node you know");
            var ip = Console.ReadLine();
            Console.WriteLine("Please enter the Name of a node you know");
            var name = Console.ReadLine();
            Console.WriteLine("Please enter your name");
            var self = Console.ReadLine();
            SetConsoleCtrlHandler(handler, true);
            var peer = new Peer(name, ip, self);
            NetworkController.SelfName = self;
            NetworkController.peers.Add(peer);


            while (true)
            {


                Console.WriteLine("Please enter your command: ");
                Console.WriteLine("1 : Node discovery ");
                Console.WriteLine("2 : Send transaction ");
                Console.WriteLine("3 : Send Block");



                switch (Console.ReadLine())
                {
                    case ("1"):
                        //TODO SEND GET NODE
                        NetworkController.AddPeer(NetworkController.SelfName, GetLocalIPAddress());
                        break;
                    case ("2"):
                        Console.WriteLine("Please enter with whom you made the transaction;");
                        var to = Console.ReadLine();
                        Console.WriteLine("Please enter the amount");
                        var amount = Decimal.Parse(Console.ReadLine());
                        NetworkController.AddTransAction(NetworkController.SelfName, to, amount);
                        break;
                    case ("3"):
                        Console.WriteLine("Please enter data");
                        var data = Console.ReadLine();
                        NetworkController.AddBlock(NetworkController.Blocks.Count, "05050505", data, NetworkController.Blocks.Last().hash);
                        break;
                    default:
                        Console.WriteLine("Invalid input detected");
                        break;
                }
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Console window closing, death imminent");
                foreach (var p in NetworkController.peers)
                {
                    p.Close();
                }
            }
            return false;
        }
        static ConsoleEventDelegate handler;  

        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

    }
}

