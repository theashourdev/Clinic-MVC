using System.Security.Cryptography;
using System.Text;
namespace Cilinc_System.Helpers
{
    public static class IdProtector
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("A123456789012345");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("B123456789012345");

        public static string EncryptId(int id)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var idBytes = BitConverter.GetBytes(id);

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var encryptedBytes = encryptor.TransformFinalBlock(idBytes, 0, idBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        public static int DecryptId(string encryptedId)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var encryptedBytes = Convert.FromBase64String(encryptedId);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return BitConverter.ToInt32(decryptedBytes, 0);
        }
    }
}
