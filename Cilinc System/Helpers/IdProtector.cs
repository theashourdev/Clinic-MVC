using System.Security.Cryptography;
using System.Text;
namespace Cilinc_System.Helpers
{
    public static class IdProtector
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("A123456789012345A123456789012345");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("B123456789012345");

        public static string EncryptId(int id)
        {
            var plainText = $"ID-{id}-XYZ123";
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(encryptedBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public static int DecryptId(string encryptedId)
        {
            encryptedId = encryptedId
                .Replace("-", "+")
                .Replace("_", "/");

            switch (encryptedId.Length % 4)
            {
                case 2: encryptedId += "=="; break;
                case 3: encryptedId += "="; break;
            }

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var encryptedBytes = Convert.FromBase64String(encryptedId);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            var decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            var parts = decryptedText.Split('-');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int id))
                return id;

            throw new InvalidOperationException("Invalid encrypted id");
        }
    }
}
