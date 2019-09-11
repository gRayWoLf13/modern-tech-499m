using System.Collections.Generic;
using System.Linq;

namespace modern_tech_499m
{
    static class Extensions
    {
        public static IEnumerable<T> Cycle<T> (this IEnumerable<T> list, int index = 0)
        {
            var count = list.Count();
            index %= count;
            while(true)
            {
                yield return list.ElementAt(index);
                index = (index + 1) % count;
            }
        }
    }
}