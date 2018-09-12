
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using P2P_Blockchain.Model;

namespace P2P_Blockchain
{
	public class NetworkController
    {
		private readonly SortedSet<Peer> peers;
		public static  int Port = 1337;

		public void AddPeer(Peer p)
		{
			this.peers.Add(p);
		}

		public void RemovePeerByIp(string ip)
		{
			foreach(var p in peers) {
				if(ip == p.IPAdress) {
					peers.Remove(p);
					return;
				}
			}
		}


    }
}
