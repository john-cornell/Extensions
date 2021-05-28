using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class TypeExtensionMethods
{
    public static bool Has<TAttribute>(this Type me) where TAttribute : Attribute
    {
        return me.CustomAttributes.Or(
            a => a.AttributeType == typeof(TAttribute)
            );
    }
}