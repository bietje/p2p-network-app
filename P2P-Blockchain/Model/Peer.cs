using Newtonsoft.Json;
using P2P_Blockchain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace P2P_Blockchain.Model
{
    public class Peer : IComparable<Peer>, IDisposable
    {
        public string Name { get; set; }
        public string IPadress { get; set; }
        private string privateAESKey { get; set; }
        private StreamWriter writer { get; set; }
        private StreamReader reader { get; set; }

        private TcpClient client;

        public Peer(string Name, string IPadress)
        {
            try
            {
                this.Name = Name;
                this.IPadress = IPadress;
                if (IPadress != NetworkController.SelfIp && NetworkController.peers.SingleOrDefault(x => x.IPadress == IPadress) == null)
                {

                    client = new TcpClient();
                    client.Connect(IPadress, NetworkController.Port);
                    NetworkStream stream = client.GetStream();
                    writer = new StreamWriter(stream, Encoding.ASCII);
                    reader = new StreamReader(stream, Encoding.ASCII);

                    Console.WriteLine($"Client Connected to {Name} on {IPadress}");
                    Command c = new Command(CommandId.SendRSA, "RSAKEYSAREAWESOME"); //TODO GET RSAKEY
                    Command encrypted = c; //TODO ENCRYPT IT
                    writer.WriteLine(JsonConvert.SerializeObject(encrypted));
                    writer.Flush();

                    string encryptedWithRSA = reader.ReadLine();

                    string decryptedAESKey = encryptedWithRSA; //TODO DECRYPT

                    Command command = JsonConvert.DeserializeObject<Command>(decryptedAESKey);
                    if (command.Cmd == CommandId.SendAES)
                    {
                        privateAESKey = command.Data;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Sascha FUCKUP Counter +1");
            }
        }

        public void SendTransaction(Transaction t)
        {

            string transaction = JsonConvert.SerializeObject(t);
            Command command = new Command(CommandId.Block, transaction);
            string c = JsonConvert.SerializeObject(command);
            writer.WriteLine(c);
            writer.Flush();
        }

        public void SendBlock(Block b)
        {
            string block = JsonConvert.SerializeObject(b);
            Command command = new Command(CommandId.Block, block);
            string c = JsonConvert.SerializeObject(command);
            writer.WriteLine(c);
            writer.Flush();
        }

        public SortedSet<Peer> SendPeer(Peer p)
        {

            string peer = JsonConvert.SerializeObject(p);
            Command command = new Command(CommandId.NodeList, peer);
            string c = JsonConvert.SerializeObject(command);

            try
            {
                writer.WriteLine(c);
                writer.Flush();
                string str = reader.ReadLine();

                SortedSet<Peer> peers = JsonConvert.DeserializeObject<SortedSet<Peer>>(str);
                return peers;
            }
            catch (Exception)
            {
                return new SortedSet<Peer>();
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
            Peer item = obj as Peer;

            if (item == null)
            {
                return false;
            }

            return (IPadress == item.IPadress);
        }

        public void Dispose()
        {
            if (client.Connected)
            {


                writer.Close();
                reader.Close();
                client.Close();
            }
        }
    }
}
