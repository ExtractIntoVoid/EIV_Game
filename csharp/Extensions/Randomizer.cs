using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExtractIntoVoid.Extensions
{
    public static class Randomizer
    {
        public static T GetRandom<T>(this IList<T> list)
        {
            var ret = RandomNumberGenerator.GetInt32(0, list.Count()-1);
            return list[ret];
        }
    }
}
