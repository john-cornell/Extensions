using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class IntExtensions
    {
        public static void EnumerateTo(this int me, Action<int> action)
        {
            Enumerable.Range(0, me).ForEach(i => action(i));
        }
    }
}
