using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hcb.Insights.Test
{
    [TestClass]
    public class Crypto_Test
    {
        [TestMethod]
        public void GetKey_01()
        {
            string result = Hcb.Insights.Crypto.GetKey();
            Assert.AreEqual(16, result.Length);
            Assert.AreEqual(true, result.EndsWith(')'));
        }
        [TestMethod]
        public void Encrypt_01()
        {
            string str = "Abc123)(*";
            string result = Hcb.Insights.Crypto.Encrypt(str);
        }
        public void EncryptDecrypt_01()
        {
            string str = "Abc123)(*";
            string result = Hcb.Insights.Crypto.Encrypt(str);
            result = Hcb.Insights.Crypto.Decrypt(result);
            Assert.AreEqual(str, result);
        }
    }
}
