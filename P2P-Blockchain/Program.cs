using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using P2P_Blockchain.Model;

namespace P2P_Blockchain
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Please enter your command: ");
            Console.WriteLine("1 : Node discovery ");
            Console.WriteLine("2 : Send transaction ");
            Console.WriteLine("3 : Send Block");
            var peer = new Peer("NAME", "192.168.1.1");
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
        }

        
    }
}
