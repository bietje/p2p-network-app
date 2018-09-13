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
            To = to;
            Amount = amount;

        }

        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }

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
                    this.Amount.Equals(item.Amount));
        }

        public override int GetHashCode()
        {
            return From.GetHashCode() +
                   To.GetHashCode() +
                   Amount.GetHashCode();
        }
    }
}
