using Newtonsoft.Json;
using P2P_Blockchain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace P2P_Blockchain.Model
{
    public class Peer : IComparable<Peer>
    {
        public string Name { get; set; }
        public string IPadress { get; set; }

        private TcpClient client;

        public Peer(string Name, string IPadress)
        {
            try
            {
                this.Name = Name;
                this.IPadress = IPadress;
                client = new TcpClient();
                client.Connect(IPadress, NetworkController.Port);
                Console.WriteLine($"Client Connected to {Name} on {IPadress}");
            }
            catch (Exception)
            {
            }
        }

        public void SendTransaction(Transaction t)
        {

            string transaction = JsonConvert.SerializeObject(t);
            Command command = new Command(CommandId.Block, transaction);
            string c = JsonConvert.SerializeObject(command);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(c);
            writer.Flush();
        }

        public void SendBlock(Block b)
        {
            string block = JsonConvert.SerializeObject(b);
            Command command = new Command(CommandId.Block, block);
            string c = JsonConvert.SerializeObject(command);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(c);
            writer.Flush();
        }

        public void SendPeer(Peer p)
        {

            string peer = JsonConvert.SerializeObject(p);
            Command command = new Command(CommandId.NodeList, peer);
            string c = JsonConvert.SerializeObject(command);

            try
            {
                NetworkStream stream = client.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader = new StreamReader(stream);

                writer.WriteLine(c);
                writer.Flush();
                string str = reader.ReadLine();

                var peers = JsonConvert.DeserializeObject<SortedSet<Peer>>(str);
                foreach (var pe in peers)
                {
                    NetworkController.peers.Add(pe);
                }


                NetworkController.ForwardPeer(p);
            }
            catch (Exception e)
            {
            }

        }

        public void Close()
        {
            client.Close();
        }

        public int CompareTo(Peer other)
        {
            if (IPadress != other.IPadress || other.IPadress.Equals(NetworkController.SelfIp))
            {
                return -1;
            }

            return 0;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Peer;

            if (item == null)
            {
                return false;
            }

            return (this.IPadress == item.IPadress);
        }
    }
}
