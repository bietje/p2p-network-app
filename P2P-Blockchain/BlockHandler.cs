using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using P2P_Blockchain.Model;

namespace P2P_Blockchain
{
    public static class BlockHandler
    {
        public static void SaveBlock(Block b)
        {
            var filename = "block" + b.id + ".txt";
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, b);
            }
        }

        public static Block LoadBlock(int id)
        {
            var filename = "block" + id + ".txt";
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (Block)binaryFormatter.Deserialize(stream);
            }

        }
    }
}
