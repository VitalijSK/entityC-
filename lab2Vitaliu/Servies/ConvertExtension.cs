using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace lab2Vitaliu.Servies
{
    public static class ConvertExtension
    {
        public static T DeserializeBuffer<T>(this byte[] buffer)
        {
            T result;
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(buffer))
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Decompress, true))
                {
                    result = (T)formatter.Deserialize(ds);
                    return result;
                }
            }
        }

        public static byte[] SerializeToBuffer<T>(this T data)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))
                {
                    formatter.Serialize(ds, data);

                }
                ms.Position = 0;
                return ms.GetBuffer();
            }
        }
    }
}
