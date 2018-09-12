using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
    [Serializable]
    public class Transaction
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Amount { get; set; }
        public string Username { get; set; }
    }
}
