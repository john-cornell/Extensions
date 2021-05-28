using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Avance
{
    public static class FileStreamExtensionMethods
    {
        public static byte[] ReadBytesFromStream(this FileStream me, int offset, int count, out int bytesActuallyRead)
        {
            byte[] output = new byte[count];

            me.Position = offset;

            bytesActuallyRead = 0;

            do
            {
                bytesActuallyRead += me.Read(output, bytesActuallyRead, count - bytesActuallyRead);
            }
            while (bytesActuallyRead != count && me.Position != me.Length);

            return output;
        }
    }
}
