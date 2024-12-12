using System.Security.Cryptography;

namespace BusinessLayer.Config
{
    public class EncryptWithAes
    {
        private static readonly byte[] Key = Convert.FromBase64String("SCXNGluWkuWXTz4+mpT6S/noo8YgQK11mLm9s95u3Jk=");
        private static readonly byte[] Iv = Convert.FromBase64String("gqdUUTZO5KyvhBKmLOTcOw==");

        public string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = Iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = Iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
