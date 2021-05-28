using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class Enums
    {
        public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            enumerator.Reset();
        }

        public static IEnumerable<T> Get<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
