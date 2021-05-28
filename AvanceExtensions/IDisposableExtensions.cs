using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class IDisposableExtensions
    {
        public static void Using(this IDisposable me, Action action)
        {
            using (me)
            {
                action();
            }
        }

        public static void Using<TDisposable>(this TDisposable me, Action<TDisposable> action) where TDisposable : IDisposable
        {
            using (me)
            {
                action(me);
            }
        }
    }
}
