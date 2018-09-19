using Newtonsoft.Json;
using P2P_Blockchain.Model;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using P2P_Blockchain.Enums;

namespace P2P_Blockchain
{
    public class Server
    {
        private readonly TcpListener _server;

        public delegate void PeerToPeerEventHandler(object sender, PeerToPeerEventArgs e);
        //public event PeerToPeerEventHandler PeerToPeerEvent;

        public Server()
        {
            _server = new TcpListener(IPAddress.Any, NetworkController.Port);

            try
            {
                _server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Michel FUCKUP Counter +1");
                Console.WriteLine("Shit just went south.. HALP!");
                Console.WriteLine(e.Message);
            }
        }

        public void Start()
        {
            Thread t = new Thread(async () =>
            {
                while (true)
                {
                    TcpClient sock = await _server.AcceptTcpClientAsync().ConfigureAwait(false);

                    ClientHandler handler = new ClientHandler();
                    Thread tp = new Thread(new ParameterizedThreadStart(handler.Run));
                    tp.Start(sock);
                }
            });

            t.Start();
        }

        public class ClientHandler
        {
            private TcpClient client;
            private bool encrypted = false;
            private string publicRSAKey = "";
            private string superduperprivateAESkey="";
            public ClientHandler()
            {
            }

            public async void Run(object client)
            {
                StreamReader reader;
                StreamWriter writer;

                this.client = client as TcpClient;
                if (this.client == null)
                {
                    Console.WriteLine("Invalid client...");
                    Console.WriteLine("Michel FUCKUP Counter +1");
                }

                string addres = ((IPEndPoint)this.client.Client.RemoteEndPoint).Address.ToString();
                reader = new StreamReader(this.client.GetStream(), Encoding.ASCII);
                writer = new StreamWriter(this.client.GetStream(), Encoding.ASCII);

                while (true)
                {
                    try
                    {
                        string received = await reader.ReadLineAsync();
                        Console.WriteLine("Server " + received);

                        try
                        {
                            if (!encrypted)
                            {
                                Command command = JsonConvert.DeserializeObject<Command>(received);
                                if (command.Cmd == CommandId.SendRSA)
                                {
                                    publicRSAKey = command.Data;
                                    superduperprivateAESkey =
                                        "AWESOMESUPERDUPERDYDUPERSECURESTRINGYTHINGTHATMICHELSHOULDDEFINITELYREPLACE";

                                    var returnCommand =
                                        JsonConvert.SerializeObject(new Command(CommandId.SendAES,
                                            superduperprivateAESkey));
                                    //ENCRYPT IT
                                    var encryptedCommand = returnCommand;

                                    writer.WriteLine(encryptedCommand);
                                    writer.Flush();

                                    encrypted = true;
                                }
                            }
                            else
                            {
                                //TODO ADD SOME ENCRYPTION SHIT
                                Command command = Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(received);

                                switch (command.Cmd)
                                {
                                    case Enums.CommandId.Transaction:
                                        NetworkController.ForwardTransaction(
                                            JsonConvert.DeserializeObject<Transaction>(command.Data));
                                        break;

                                    case Enums.CommandId.Block:
                                        NetworkController.ForwardBlock(
                                            JsonConvert.DeserializeObject<Block>(command.Data));
                                        break;

                                    case Enums.CommandId.Disconnect:
                                        Console.WriteLine("NOT YET IMPLEMENTED!");
                                        break;

                                    case Enums.CommandId.NodeList:
                                        string serialized = JsonConvert.SerializeObject(NetworkController.peers);
                                        writer.WriteLine(serialized);
                                        writer.Flush();
                                        Peer peer = JsonConvert.DeserializeObject<Peer>(command.Data);
                                        NetworkController.peers.Add(peer);
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Oepsie, iets met JSON.. Of iets totaal anders..");
                            Console.WriteLine("Michel FUCKUP Counter +1");
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                            this.client.Close();
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("DISCONNECTED ");
                        this.client.Close();
                        return;

                    }

                }
            }
        }
    }
}
