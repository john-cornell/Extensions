using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avance
{
    public static class NullableExtensions
    {
        public static bool AssertHasValue<T>(this Nullable<T> me, Action<string> failback, Type callingType, string method, string extraDetails)  where T : struct
        {
            if (me.HasValue) return true;

            failback(string.Concat("Failed HasValue Assert for '", typeof(T), "' at '", callingType.ToString(), ".", method, "'. ", extraDetails));

            return false;
        }

        public static bool AssertHasValue<T>(this Nullable<T> me, Action<string> failback, Type callingType, string method) where T : struct
        {            
            return me.AssertHasValue(failback, callingType, method, string.Empty);
        }

        public static bool NullOr<T>(this Nullable<T> me, T comparison) where T : struct
        {
            return !me.HasValue ||
                    me.Equals(comparison);
        }

        public static bool NullOr<T>(this Nullable<T> me, Predicate<T> predicate) where T : struct
        {
            return !me.HasValue ||
                predicate(me.Value);
        }
    }
}
