using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.Helpers
{
    public class StreamHelper
    {
        public static MemoryStream StringToStream(string input)
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(input);
            streamWriter.Flush();
            memoryStream.Position = 0;

            return memoryStream;
        }

        public static MemoryStream ByteArrayToStream(byte[] input)
        {
            MemoryStream memoryStream = new MemoryStream();

            memoryStream.Write(input, 0, input.Length);

            return memoryStream;
        }

        public static byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
