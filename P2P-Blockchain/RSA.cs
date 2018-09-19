using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace P2P_Blockchain
{
	public class RSA
	{
		public static KeyPair GenerateKeyPair()
		{
			KeyPair kp = null;

			try {
				var csp = new CspParameters {
					ProviderType = 1,
					Flags = CspProviderFlags.UseArchivableKey,
					KeyNumber = (int) KeyNumber.Exchange
				};

				var provider = new RSACryptoServiceProvider(csp);

				kp = new KeyPair {
					PublicKey = provider.ToXmlString(false),
					PrivateKey = provider.ToXmlString(true)
				};

			} catch (Exception ex) {
				Debug.WriteLine("Unable to generete RSA keypair");
				throw ex;
			}

			return kp;
		}

		public static byte[] Encrypt(KeyPair kp, string text)
		{
			var csp = new CspParameters {ProviderType = 1};

			var provider = new RSACryptoServiceProvider(csp);
			provider.FromXmlString(kp.PublicKey);
			var bytes = Encoding.ASCII.GetBytes(text);

			return provider.Encrypt(bytes, false);
		}

		public static string Decrypt(KeyPair kp, string data)
		{
			var csp = new CspParameters {ProviderType = 1};
			var provider = new RSACryptoServiceProvider(csp);

			provider.FromXmlString(kp.PrivateKey);
			var bytes = Encoding.ASCII.GetBytes(data);
			var plain = provider.Decrypt(bytes, false);
			return Encoding.ASCII.GetString(plain);
		}
	}
}
