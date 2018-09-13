using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using P2P_Blockchain.Model;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace P2P_Blockchain
{
	public class Server
	{
		private readonly TcpListener _server;

		public delegate void PeerToPeerEventHandler(object sender, PeerToPeerEventArgs e);
		//public event PeerToPeerEventHandler PeerToPeerEvent;

		public Server()
		{
			this._server = new TcpListener(IPAddress.Any, NetworkController.Port);

			try {
				this._server.Start();
			} catch(Exception e) {
				Console.WriteLine("Shit just went south.. HALP!");
				Console.WriteLine(e.Message);
			}
		}

		public void Start()
		{
			var t = new Thread(async () => {
				while(true) {
					var sock = await _server.AcceptTcpClientAsync().ConfigureAwait(false);

					var handler = new ClientHandler();
					Thread tp = new Thread(new ParameterizedThreadStart(handler.Run));
					tp.Start(sock);
				}
			});

			t.Start();
		}

		public class ClientHandler
		{
			private TcpClient client;

			public ClientHandler()
			{
			}

			public async void Run(object client)
			{
				StreamReader reader;
				StreamWriter writer;

				this.client = client as TcpClient;
				if(this.client == null) {
					Console.WriteLine("Invalid client...");
				}

			    var addres = ((IPEndPoint) this.client.Client.RemoteEndPoint).Address.ToString();


                while (true) {
                    try
                    {


                        reader = new StreamReader(this.client.GetStream(), Encoding.ASCII);
                        writer = new StreamWriter(this.client.GetStream(), Encoding.ASCII);

                        char[] bytes = new char[2048];
                        var num = await reader.ReadAsync(bytes, 0, bytes.Length);
                        string received = new string(bytes);
                        Console.WriteLine("Server " + received);

                        try
                        {
                            Command command = Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(received);

                            switch (command.Cmd)
                            {
                                case Enums.CommandId.Transaction:
                                    NetworkController.ForwardTransaction(
                                        JsonConvert.DeserializeObject<Transaction>(command.Data));
                                    break;

                                case Enums.CommandId.Block:
                                    Block b = JsonConvert.DeserializeObject<Block>(command.Data);
                                    NetworkController.ForwardBlock(b);
                                    BlockHandler.SaveBlock(b);
                                    break;

                                case Enums.CommandId.Disconnect:
                                    Console.WriteLine("NOT YET IMPLEMENTED!");
                                    break;

                                case Enums.CommandId.NodeList:
                                    var peer = JsonConvert.DeserializeObject<Peer>(command.Data);
                                    var peers = NetworkController.peers;
                                    var serialized = JsonConvert.SerializeObject(peers);
                                    NetworkController.peers.Add(peer);
                                    await writer.WriteAsync(serialized);
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Oepsie, iets met JSON.. Of iets totaal anders..");
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("DISCONNECTED ");
                        return;
                        
                    }

                }
			}
		}
	}
}
