using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace P2P_Blockchain
{
    public class PeerToPeerEventArgs : EventArgs 
    {
		public PeerToPeerEventArgs(string result)
		{
			this.Result = result;
		}

		public string Result { get; }
    }
}
