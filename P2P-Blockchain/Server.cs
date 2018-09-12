using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using P2P_Blockchain.Model;
using System.Threading;
using System.IO;

namespace P2P_Blockchain
{
	class Server
	{
		private readonly TcpListener _server;

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
				if(client == null) {
					Console.WriteLine("Invalid client...");
				}

				while(true) {
					reader = new StreamReader(this.client.GetStream(), Encoding.ASCII);
					writer = new StreamWriter(this.client.GetStream(), Encoding.ASCII);

					char[] bytes = new char[2048];
					var num =await reader.ReadAsync(bytes, 0, bytes.Length);
					string received = new string(bytes);
					Console.WriteLine(received);
				}
			}
		}
	}
}
