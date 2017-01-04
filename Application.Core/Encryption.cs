using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace NCC.ClearView.Application.Core
{
	public class Encryption
	{
		private string hashAlgorithm = "SHA1";
		private int passwordIterations = 2;
		private int keySize = 256;
//        private string initVector = "A@8G#dg34nm^%$YWT4%$3qFAS#$JC#&gt;R$FMsd156fdsa%^TUT(DFga";
//        private string initVector = "$1B2c3D*e1F6g4H8";

        private string saltValue = "nR$F%UTFAS#As$JC#&m^gt;56fda@g34%$d#Msda%^TD8GYW($3qT41Fg";
        private string initVector = "h6g9&82c31F1D*eB";

		public Encryption()
		{
		}
		public string Encrypt(string plainText, string passPhrase)
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);
			byte[] plainTextBytes  = Encoding.UTF8.GetBytes(plainText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
			byte[] keyBytes = password.GetBytes(keySize / 8);
			RijndaelManaged symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;        
			ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
			MemoryStream memoryStream = new MemoryStream();        
			CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
			cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] cipherTextBytes = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			string cipherText = Convert.ToBase64String(cipherTextBytes);
			return cipherText;
		}

		public string Decrypt(string cipherText, string passPhrase)
		{
			byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
			byte[] saltValueBytes  = Encoding.ASCII.GetBytes(saltValue);
			byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
			PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
			byte[] keyBytes = password.GetBytes(keySize / 8);
			RijndaelManaged    symmetricKey = new RijndaelManaged();
			symmetricKey.Mode = CipherMode.CBC;
			ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
			MemoryStream  memoryStream = new MemoryStream(cipherTextBytes);
			CryptoStream  cryptoStream = new CryptoStream(memoryStream, decryptor,CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];
			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
			return plainText;
		}
	}
}
