using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static void SafeAdd<TKey, TValue>(this ConcurrentDictionary<TKey, List<TValue>> me, TKey key, TValue value)
        {
            if (!me.ContainsKey(key)) me[key] = new List<TValue>();

            if (!me[key].Contains(value)) me[key].Add(value);
        }

        public static void CleanRemove<TKey, TValue>(this ConcurrentDictionary<TKey, List<TValue>> me, TKey key, TValue value)
        {
            if (!me.ContainsKey(key)) return;

            if (!me[key].Contains(value)) return;

            me[key].Remove(value);

            List<TValue> stuff;

            if (me[key].Count == 0) me.TryRemove(key, out stuff);
        }

        public static void CleanRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> me, TKey key)
        {
            if (!me.ContainsKey(key)) return;

            TValue stuff;

            me.TryRemove(key, out stuff);
        }

        public static bool ListContains<TKey, TValue>(this ConcurrentDictionary<TKey, List<TValue>> me, TKey key, TValue value)
        {
            return me.ContainsKey(key) && me[key].Contains(value);
        }

        public static IEnumerable<TValue> FromList<TKey, TValue>(this ConcurrentDictionary<TKey, List<TValue>> me, TKey key)
        {
            if (!me.ContainsKey(key)) yield break;

            List<TValue> list = null;

            try
            {
                list = me[key];
            }
            catch (NullReferenceException) { };//protect against race conditions removing list from dictionary

            if (list == null) yield break;

            foreach (TValue value in list)
            {
                yield return value;
            }
        }

        public static TValue SafeGet<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> me, TKey key)
        {
            if (!me.ContainsKey(key)) return default(TValue);

            return me[key];
        }
    }
}
