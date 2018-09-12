using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
    [Serializable]
    public class Block
    {
        public int id { get; set; }
        public string nonce { get; set; }
        public string data { get; set; }
        public string hash { get; set; }
        public string previous { get; set; }
        public string Username { get; set; }
    }
}
