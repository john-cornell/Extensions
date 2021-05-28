using System;
using System.Threading;

namespace Avance
{
    public static class FuncExtensions
    {
        public static bool TryExecute<T>(this Func<T> func, int timeout, out T result)
        {
            T t = default(T);

            Thread thread = new Thread(() => t = func());

            thread.IsBackground = true;

            thread.Start();

            bool completed = thread.Join(timeout);

            if (!completed) thread.Abort();

            result = t;

            return completed;
        }
    }
}
