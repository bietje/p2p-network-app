using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using P2P_Blockchain.Model;

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
	}
}
