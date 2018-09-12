using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
    [Serializable]
    public class Block
    {
        public Block(int id, string nonce, string data,  string previous,  string hash = "0000")
        {
            this.id = id;
            this.nonce = nonce;
            this.data = data;
            this.hash = hash;
            this.previous = previous;
            Username = NetworkController.SelfName;
        }

        public int id { get; set; }
        public string nonce { get; set; }
        public string data { get; set; }
        public string hash { get; set; }
        public string previous { get; set; }
        public string Username { get; set; }
    }
}
