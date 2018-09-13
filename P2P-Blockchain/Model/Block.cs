using System;
using System.Collections.Generic;
using System.Text;

namespace P2P_Blockchain.Model
{
    [Serializable]
    public class Block : IComparable<Block>
    {
        public Block(int id, string nonce, string data,  string previous,  string hash = "0000")
        {
            this.id = id;
            this.nonce = nonce;
            this.data = data;
            this.hash = hash;
            this.previous = previous;
        }

        public int id { get; set; }
        public string nonce { get; set; }
        public string data { get; set; }
        public string hash { get; set; }
        public string previous { get; set; }

        public int CompareTo(Block other)
        {
            return this.id.CompareTo(other.id);
        }

        public override bool Equals(object obj)
        {
            var item = obj as Block;

            if (item == null)
            {
                return false;
            }

            return (this.id.Equals(item.id) &&
                    this.nonce.Equals(item.nonce) &&
                    this.data.Equals(item.data) &&
                    this.hash.Equals(item.hash) &&
                    this.previous.Equals(item.previous));
        }

        public override int GetHashCode()
        {
            return id.GetHashCode() +
                   nonce.GetHashCode() +
                   data.GetHashCode() +
                   hash.GetHashCode() +
                   previous.GetHashCode();
        }
    }
}