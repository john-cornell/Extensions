using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExceptionExtension
{
    public static Exception GetInnerMostException(this Exception me)
    {
        if (me == null) return null;

        Exception inner = me;

        while (inner.InnerException != null)
        {
            inner = inner.InnerException;
        }

        return inner;
    }
}