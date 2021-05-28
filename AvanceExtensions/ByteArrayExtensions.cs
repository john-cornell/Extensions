using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avance
{
    public static class ByteArrayExtensions
    {
        public static bool RangeEquals(this byte[] me, byte[] bytes, int myOffset, int bytesOffset, int count)
        {
            bool equal = true;

            Enumerable
                .Range(0, count)
                .ForEach(i => { if (bytes[i + bytesOffset] != me[i + myOffset]) equal = false; }
                );

            return equal;
        }
    }
}
