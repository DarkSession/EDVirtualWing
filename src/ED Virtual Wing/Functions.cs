using System.Security.Cryptography;
using System.Text;

namespace ED_Virtual_Wing
{
    public static class Functions
    {
        private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static string GenerateString(int length)
        {
            byte[] data = new byte[4 * length];
            using RandomNumberGenerator crypto = RandomNumberGenerator.Create();
            crypto.GetBytes(data);
            StringBuilder result = new(length);
            for (int i = 0; i < length; i++)
            {
                uint rnd = BitConverter.ToUInt32(data, i * 4);
                long idx = rnd % chars.Length;
                result.Append(chars[idx]);
            }
            return result.ToString();
        }
    }
}
