using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avance.ExtensionMethods
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> TakeAndRemove<T>(this Queue<T> me, int count)
        {
            for (int i = 0; i < Math.Min(me.Count, count); i++)
                yield return me.Dequeue();
        }
    }
}
