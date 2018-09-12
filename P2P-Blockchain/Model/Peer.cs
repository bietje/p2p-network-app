using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace P2P_Blockchain.Model
{
    public class Peer
    {
        public string Name { get; set; }
        public string IPadress { get; set; }
        public string From { get; set; }

        private TcpClient client;

        public Peer(string Name, string IPadress, string From)
        {
            this.From = From;
            this.Name = Name;
            this.IPadress = IPadress;
            client = new TcpClient();
            client.Connect(IPadress, NetworkController.Port);
            Console.WriteLine($"Connected to {Name} on {IPadress}");
        }

        public void SendTransaction(Transaction t)
        {

            var transaction = JsonConvert.SerializeObject(t);            
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(transaction);
            writer.Flush();
        }

        public void SendBlock(Block b)
        {
            var block = JsonConvert.SerializeObject(b);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(block);
            writer.Flush();
        }

        public void SendPeer(Peer p)
        {
            var peer = JsonConvert.SerializeObject(p);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(peer);
            writer.Flush();
        }        

    }
}
