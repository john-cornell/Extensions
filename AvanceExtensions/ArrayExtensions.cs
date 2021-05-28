using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avance
{
    public static class ArrayExtensions
    {
        static Random _random = new Random((int)DateTime.Now.Ticks);

        static ArrayExtensions()
        {

        }

        public static void Loop<T>(this T[] me, Action<T> action)
        {
            for (int i = 0; i < me.Length; i++)
            {
                action(me[i]);
            }
        }

        public static T GetRandom<T>(this T[] me)
        {
            int index = _random.Next(me.Length);

            return me[index];
        }

        private static T[] CopySlice<T>(this T[] source, int index, int length, bool padToLength = false)
        {
            int n = length;
            T[] slice = null;

            if (source.Length < index + length)
            {
                n = source.Length - index;

                if (padToLength)
                {
                    slice = new T[length];
                }
            }

            if (slice == null) slice = new T[n];
            Array.Copy(source, index, slice, 0, n);
            return slice;
        }

        public static IEnumerable<T[]> Chunk<T>(this T[] source, int count, bool padToLength = false)
        {
            for (var i = 0; i < source.Length; i += count)
            {
                yield return source.CopySlice(i, count, padToLength);
            }

        }
    }
}
