using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
    [Serializable]
    public class Transaction
    {
        public Transaction(string from, string to, decimal amount)
        {
            From = from;
            To = to;
            Amount = amount;
            Username = NetworkController.SelfName;
        }

        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
        public string Username { get; set; }
    }
}
