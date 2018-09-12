
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
        public static string SelfName = "";
        public SortedSet<Block> Blocks { get; set; }
        public SortedSet<Transaction> Transactions { get; set; }

        public void AddPeer(string name, string IPadress)
        {
            var peer = new Peer(name, IPadress, SelfName);
            if (peers.Add(peer))
            {
                foreach (var p in peers)
                {
                    p.SendPeer(peer);   
                }
            }
        }

        public void AddBlock(int id, string nonce, string data, string previous)
        {
            var block = new Block(id, nonce, data, previous);            
            if (Blocks.Add(block))
            {
                foreach (var p in peers)
                {
                    p.SendBlock(block);
                }
            }
        }

        public void AddTransAction(string from, string to, decimal amount)
        {
            var transaction = new Transaction(from, to, amount);
            if (Transactions.Add(transaction))
            {
                foreach (var p in peers)
                {
                    p.SendTransaction(transaction);
                }
            }
        }

        public void RemovePeer(string name)
        {            
        }

    }
}
