using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


public static class HttpPostedFileBaseExtensions
{
    public static byte[] ToByteArray(this HttpPostedFileBase me)
    {
        byte[] data;
        using (Stream inputStream = me.InputStream)
        {
            MemoryStream memoryStream = inputStream as MemoryStream;
            if (memoryStream == null)
            {
                memoryStream = new MemoryStream();
                inputStream.CopyTo(memoryStream);
            }
            data = memoryStream.ToArray();
        }

        return data;
    }
}