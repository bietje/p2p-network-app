using P2P_Blockchain.Model;
using System.Collections.Generic;

namespace P2P_Blockchain
{
    public static class NetworkController
    {
        public static SortedSet<Peer> peers = new SortedSet<Peer>();
        public static int Port = 1337;
        public static string SelfName = "";
        public static string SelfIp = "";
        public static SortedSet<Block> Blocks { get; set; } = new SortedSet<Block>();
        public static SortedSet<Transaction> Transactions { get; set; } = new SortedSet<Transaction>();

        public static void AddPeer(string name, string IPadress)
        {
            Peer peer = new Peer(name, IPadress);

            SortedSet<Peer> tempPeer = new SortedSet<Peer>(NetworkController.peers);
            foreach (Peer p in peers)
            {
                SortedSet<Peer> temptemp = new SortedSet<Peer>(p.SendPeer(peer));
                foreach (Peer pe in temptemp)
                {
                    if (pe.IPadress != NetworkController.SelfIp)
                    {
                        tempPeer.Add(pe);
                    }

                }
            }

            foreach (Peer pee in tempPeer)
            {
                if (pee.IPadress != NetworkController.SelfIp && !NetworkController.peers.Contains(pee))
                {
                    NetworkController.peers.Add(pee);
                    //AddPeer(name, IPadress); //SHOULD THIS BE HERE?
                }

            }


        }

        //public static void ForwardPeer(Peer peer)
        //{
        //    if (peer.IPadress != SelfIp)
        //    {
        //        if (peers.Add(peer))
        //        {

        //        }
        //    }
        //}

        public static void AddBlock(int id, string nonce, string data, string previous)
        {
            Block block = new Block(id, nonce, data, previous);
            if (Blocks.Add(block))
            {
                foreach (Peer p in peers)
                {
                    p.SendBlock(block);
                }
            }
        }

        public static void ForwardBlock(Block block)
        {
            if (Blocks.Add(block))
            {
                foreach (Peer p in peers)
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
                foreach (Peer p in peers)
                {
                    p.SendTransaction(transaction);
                }
            }
        }

        public static void ForwardTransaction(Transaction transaction)
        {
            if (Transactions.Add(transaction))
            {
                foreach (Peer p in peers)
                {
                    if (p.IPadress == NetworkController.SelfIp)
                    {
                        continue;
                    }

                    p.SendTransaction(transaction);
                }
            }
        }

        public static void RemovePeer(string name)
        {

        }

        public static void Close()
        {
            foreach (Peer p in peers)
            {
                p.SendPeer(p);
            }
        }

    }
}
