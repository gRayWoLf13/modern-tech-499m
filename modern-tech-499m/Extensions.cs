using System;
using System.Collections.Generic;

namespace modern_tech_499m
{
    static class Extensions
    {
        public static IEnumerable<T> Cycle<T> (this List<T> list, int index = 0)
        {
            int count = list.Count;
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();
            while(true)
            {
                yield return list[index];
                index = (index + 1) % count;
            }
        }

        public static IEnumerable<T> ReverceCycle<T> (this List<T> list, int index = 0)
        {
            int count = list.Count;
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException();
            while (true)
            {
                yield return list[index];
                index = index == 0 ? count - 1 : index - 1;
            }
        }
    }
}