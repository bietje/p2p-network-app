using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
    [Serializable]
    public class Transaction : IComparable<Transaction>
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

        public int CompareTo(Transaction other)
        {
            return this.GetHashCode().CompareTo(other.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            var item = obj as Transaction;

            if (item == null)
            {
                return false;
            }

            return (this.From.Equals(item.From) &&
                    this.To.Equals(item.To) &&
                    this.Amount.Equals(item.Amount) &&
                    this.Username.Equals(item.Username));
        }

        public override int GetHashCode()
        {
            return From.GetHashCode() +
                   To.GetHashCode() +
                   Amount.GetHashCode() +
                   Username.GetHashCode();
        }
    }
}
