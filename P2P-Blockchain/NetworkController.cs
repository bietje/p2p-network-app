
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using P2P_Blockchain.Model;

namespace P2P_Blockchain
{
	public static class NetworkController
    {
		public static SortedSet<Peer> peers = new SortedSet<Peer>();
		public static  int Port = 1337;
        public static string SelfName = "";
        public static SortedSet<Block> Blocks { get; set; } = new SortedSet<Block>();
        public static SortedSet<Transaction> Transactions { get; set; } = new SortedSet<Transaction>();

        public static void AddPeer(string name, string IPadress)
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

        public static void ForwardPeer(Peer peer)
        {
            if (peers.Add(peer))
            {
                foreach (var p in peers)
                {
                    p.SendPeer(peer);
                }
            }
        }    

        public static void AddBlock(int id, string nonce, string data, string previous)
        {
            var block = new Block(id, nonce, data, previous);            
            if (Blocks.Add(block))
            {
                foreach (var p in peers)
                {
                    p.SendBlock(block);
                }
                SendResponse(block);
            }
        }

        public static void ForwardBlock(Block block)
        {
            if (Blocks.Add(block))
            {
                foreach (var p in peers)
                {
                    p.SendBlock(block);
                }
            }
        }

        public static void SendResponse(object obj)
        {
            foreach (var p in peers)
            {
                p.SendResponse(obj);
            }
        }


        public static void AddTransAction(string from, string to, decimal amount)
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

        public static void ForwardTransaction(Transaction transaction)
        {
            if (Transactions.Add(transaction))
            {
                foreach (var p in peers)
                {
                    p.SendTransaction(transaction);
                }
            }
        }

        public static void RemovePeer(string name)
        {            

        }

        public static void Close()
        {
            foreach (var p in peers)
            {
                p.SendPeer(p);
            }
        }

    }
}
