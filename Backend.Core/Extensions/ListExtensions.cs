using System.Collections.Generic;
using System.Security.Cryptography;

namespace Backend.Core.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            int n = list.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do
                {
                    random.GetBytes(box);
                }
                while (!(box[0] < n * (byte.MaxValue / n)));

                int k = box[0] % n;
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}