
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
	public class NetworkController
    {
		private readonly SortedSet<Peer> clients;

		public void AddPeer(Peer p)
		{
			this.clients.Add(p);
		}

		public void RemovePeerByIp(string ip)
		{
			foreach(var p in clients) {
				if(ip == p.IPAdress) {
					clients.Remove(p);
					return;
				}
			}
		}


    }
}
