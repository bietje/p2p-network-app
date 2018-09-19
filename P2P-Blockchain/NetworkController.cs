using P2P_Blockchain.Model;
using System.Collections.Generic;

namespace P2P_Blockchain
{
    public static class NetworkController
    {
        public static SortedSet<TeringClientPeer> peers = new SortedSet<TeringClientPeer>();
        public static int Port = 1337;
        public static string SelfName = "";
        public static string SelfIp = "";
        public static SortedSet<Block> Blocks { get; set; } = new SortedSet<Block>();
        public static SortedSet<Transaction> Transactions { get; set; } = new SortedSet<Transaction>();

        public static void AddPeer(string name, string IPadress)
        {
            TeringClientPeer peer = new TeringClientPeer(name, IPadress);

            SortedSet<TeringClientPeer> tempPeer = new SortedSet<TeringClientPeer>(NetworkController.peers);
            foreach (TeringClientPeer p in peers)
            {
                SortedSet<TeringClientPeer> temptemp = new SortedSet<TeringClientPeer>(p.SendPeer(peer));
                foreach (TeringClientPeer pe in temptemp)
                {
                    if (pe.IPadress != NetworkController.SelfIp)
                    {
                        tempPeer.Add(pe);
                    }
                }
            }

            foreach (TeringClientPeer p in tempPeer)
            {
                if (p.IPadress != SelfIp && !peers.Contains(p))
                {
                    peers.Add(p);
                }
            }
        }

        public static void AddBlock(int id, string nonce, string data, string previous)
        {
            Block block = new Block(id, nonce, data, previous);
            if (Blocks.Add(block))
            {
                foreach (TeringClientPeer p in peers)
                {
                    p.SendBlock(block);
                }
            }
        }

        public static void ForwardBlock(Block block)
        {
            if (Blocks.Add(block))
            {
                foreach (TeringClientPeer p in peers)
                {
                    p.SendBlock(block);
                }
            }
        }


        public static void AddTransAction(string from, string to, decimal amount)
        {
            Transaction transaction = new Transaction(from, to, amount);
            if (Transactions.Add(transaction))
            {
                foreach (TeringClientPeer p in peers)
                {
                    p.SendTransaction(transaction);
                }
            }
        }

        public static void ForwardTransaction(Transaction transaction)
        {
            if (Transactions.Add(transaction))
            {
                foreach (TeringClientPeer p in peers)
                {
                    if (p.IPadress == SelfIp)
                    {
                        continue;
                    }

                    p.SendTransaction(transaction);
                }
            }
        }

    }
}
