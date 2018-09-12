using System;
using System.Collections.Generic;
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

            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            Console.WriteLine("Please enter your command: ");
            Console.WriteLine("1 : Node discovery ");
            Console.WriteLine("2 : Send transaction ");
            Console.WriteLine("3 : Send Block");
           
            s = new Server();
			s.Start();
            NetworkController.SelfName = "Sascha";

            NetworkController.AddPeer("test2", "145.49.23.96");
            NetworkController.AddTransAction("", "", 5);

            switch (Console.ReadLine())
            {
                case ("1"):
                    break;
                case ("2"):
                    break;
                case ("3"):
                    break;
                default:
                    Console.WriteLine("Invalid input detected");
                    break;
            }
            
            SetConsoleCtrlHandler(handler, true);
            Console.ReadLine();
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

