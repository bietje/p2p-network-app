using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using P2P_Blockchain.Enums;

namespace P2P_Blockchain.Model
{
    public class Peer : IComparable
    {
        public string Name { get; set; }
        public string IPadress { get; set; }
        public string From { get; set; }

        private TcpClient client;

        public Peer(string Name, string IPadress, string From)
        {
            try
            {


                this.From = From;
                this.Name = Name;
                this.IPadress = IPadress;
                client = new TcpClient();
                client.Connect(IPadress, NetworkController.Port);
                Console.WriteLine($"Client Connected to {Name} on {IPadress}");
            }
            catch (Exception e)
            {
            }
        }

        public void SendTransaction(Transaction t)
        {

            var transaction = JsonConvert.SerializeObject(t);
            var command = new Command(CommandId.Block, transaction);
            var c = JsonConvert.SerializeObject(command);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(c);
            writer.Flush();
        }

        public void SendBlock(Block b)
        {            
            var block = JsonConvert.SerializeObject(b);
            var command = new Command(CommandId.Block, block);
            var c = JsonConvert.SerializeObject(command);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(c);
            writer.Flush();
        }

        public void SendResponse(object obj)
        {
            var block = obj as Block;
            var transaction = obj as Transaction;
            if (block == null && transaction == null)
            {
                return;
            }
            var data = JsonConvert.SerializeObject(obj);
            var command = new Command(CommandId.Response, data);
            var c = JsonConvert.SerializeObject(command);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(c);
            writer.Flush();
        }

        public void SendPeer(Peer p)
        {   
            
            var peer = JsonConvert.SerializeObject(p);
            var command = new Command(CommandId.NodeList, peer);
            var c = JsonConvert.SerializeObject(command);
            try
            {
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(c);
                writer.Flush();
                byte[] data = new byte[2048];
                int numBytesRead = stream.Read(data, 0, data.Length);
                if (numBytesRead > 0)
                {
                    string str = Encoding.ASCII.GetString(data, 0, numBytesRead);
                    var peers = JsonConvert.DeserializeObject<SortedSet<Peer>>(str);
                    foreach (var pe in peers)
                    {
                        NetworkController.peers.Add(pe);
                    }

                    NetworkController.ForwardPeer(p);
                }
            }
            catch (Exception e)
            {
            }

        }

        public void Close()
        {
            client.Close();
        }

        public int CompareTo(object obj)
        {
            var p2 = (Peer) obj;

            if (this.IPadress != p2.IPadress)
                return 1;
                return 0;
        }
    }
}
