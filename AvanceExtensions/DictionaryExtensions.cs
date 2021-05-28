using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static void SafeAdd<TKey, TValue>(this Dictionary<TKey, List<TValue>> me, TKey key, TValue value)
        {
            if (!me.ContainsKey(key)) me[key] = new List<TValue>();

            if (!me[key].Contains(value)) me[key].Add(value);
        }

        public static void SafeAdd<TOuterKey, TValue, TInnerKey>(this Dictionary<TOuterKey, Dictionary<TInnerKey, TValue>> me, TOuterKey outerKey, TInnerKey innerKey, TValue innerValue)
        {
            if (!me.ContainsKey(outerKey)) me[outerKey] = new Dictionary<TInnerKey, TValue>();
            
            me[outerKey][innerKey] = innerValue;
        }

        public static void SafeAdd<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> me, TKey key, TValue value)
        {
            if (!me.ContainsKey(key)) me[key] = new HashSet<TValue>();

            if (!me[key].Contains(value)) me[key].Add(value);
        }

        public static void CleanRemove<TKey, TValue>(this Dictionary<TKey, List<TValue>> me, TKey key, TValue value)
        {
            if (!me.ContainsKey(key)) return;

            if (!me[key].Contains(value)) return;

            me[key].Remove(value);

            if (me[key].Count == 0) me.Remove(key);
        }

        public static void CleanRemove<TKey, TValue>(this Dictionary<TKey, HashSet<TValue>> me, TKey key, TValue value)
        {
            if (!me.ContainsKey(key)) return;

            if (!me[key].Contains(value)) return;

            me[key].Remove(value);

            if (me[key].Count == 0) me.Remove(key);
        }

        public static void CleanRemove<TKey, TValue>(this Dictionary<TKey, TValue> me, TKey key)
        {
            if (!me.ContainsKey(key)) return;

            me.Remove(key);
        }

        public static void CleanRemoveValue<TKey, TValue>(this Dictionary<TKey, List<TValue>> me, TValue value)
        {
            List<TKey> toRemove = new List<TKey>();

            foreach (TKey key in me.Keys)
            {
                if (me[key].Contains(value))
                {
                    me[key].Remove(value);

                    if (me[key].Count == 0) toRemove.Add(key);
                }
            }

            toRemove.ForEach(k => me.Remove(k));
        }

        public static bool ListContains<TKey, TValue>(this Dictionary<TKey, List<TValue>> me, TKey key, TValue value)
        {
            return me.ContainsKey(key) && me[key].Contains(value);
        }

        public static IEnumerable<TValue> FromList<TKey, TValue>(this Dictionary<TKey, List<TValue>> me, TKey key)
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

        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> me, TKey key)
        {
            if (!me.ContainsKey(key)) return default(TValue);

            return me[key];
        }
    }
}
