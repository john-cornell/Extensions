using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avance
{
    public static class BoolExtensions
    {
        public static string ToYesNoString(this bool me)
        {
            return me ? "Yes" : "No";
        }

        public static string ToYesNoString(this bool? me)
        {
            return me.HasValue ? (me.Value ? "Yes" : "No") : "";
        }

        public static bool Not(this bool me)
        {
            return !me;
        }
    }
}
