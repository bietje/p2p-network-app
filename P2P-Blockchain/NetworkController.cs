﻿
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using P2P_Blockchain.Model;

namespace P2P_Blockchain
{
	public class NetworkController
    {
		public static SortedSet<Peer> peers;
		public static  int Port = 1337;
        public static string selfName = "";

        public void AddPeer(string name, string IPadress)
        {
            var peer = new Peer(name, IPadress, selfName);
            if (peers.Add(peer))
            {
                foreach (var p in peers)
                {
                    p.SendPeer(peer);   
                }
            }
        }

        public void RemovePeer(string name)
        {            
        }

    }
}
