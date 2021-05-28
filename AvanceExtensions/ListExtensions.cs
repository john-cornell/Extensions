using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Avance
{
    public static class ListExtensions
    {
        public static void IndexedFor<T>(this List<T> me, Action<int, T> action)
        {
            for (int i = 0; i < me.Count; i++)
            {
                action(i, me[i]);
            }
        }

        public static T GetNew<T>(this List<T> me)
        {
            T item = Activator.CreateInstance<T>();

            me.Add(item);

            return item;
        }

        public static T GetNew<T>(this List<T> me, params object[] args) where T : class
        {
            T item = Activator.CreateInstance(typeof(T), args) as T;

            me.Add(item);

            return item;
        }

        public static void RemoveWhere<T>(this Collection<T> me, Func<T, bool> predicate)
        {
            T[] toRemove = me.Where(t => predicate(t)).ToArray();

            for (int i = 0; i < toRemove.Length; i++)
            {
                me.Remove(toRemove[i]);
            }
        }

        public static int IndexOf<T>(this List<T> me, Func<T, bool> predicate)
        {
            if (me != null)
            {
                for (int i = 0; i < me.Count; i++)
                {
                    if (predicate(me[i])) return i;
                }
            }

            return -1;
        }

        public static List<List<T>> Split<T>(this List<T> me, int maxElementsInList)
        {
            return me
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / maxElementsInList)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
