using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Hcb.Insights
{
    public class Crypto
    {
        static SecureString key = new SecureString();
        internal static string GetKey()
        {
            key.AppendChar('&');
            key.AppendChar('*');
            key.AppendChar('T');
            key.AppendChar('h');
            key.AppendChar('%');
            key.AppendChar('$');
            key.AppendChar('L');
            key.AppendChar('1');
            key.AppendChar('2');
            key.AppendChar('+');
            key.AppendChar('+');    //11
            key.AppendChar('*');
            key.AppendChar('-');
            key.AppendChar('_');
            key.AppendChar('=');
            key.AppendChar(')');    //16
            string plainStr = new System.Net.NetworkCredential(string.Empty, key).Password;
            return plainStr;
        }
        public static string Encrypt(string data)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Mode = CipherMode.ECB;
            DES.Key = Encoding.ASCII.GetBytes(GetKey());
            key.Dispose();

            DES.Padding = PaddingMode.PKCS7;
            ICryptoTransform DESEncrypt = DES.CreateEncryptor();
            Byte[] buffer = ASCIIEncoding.ASCII.GetBytes(data);

            return Convert.ToBase64String(DESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string Decrypt(string data)
        {
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Mode = CipherMode.ECB;
            DES.Key = Encoding.ASCII.GetBytes(GetKey());
            key.Dispose();

            DES.Padding = PaddingMode.PKCS7;
            ICryptoTransform DESEncrypt = DES.CreateDecryptor();
            Byte[] Buffer = Convert.FromBase64String(data.Replace(" ", "+"));

            return Encoding.UTF8.GetString(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
    }
}
