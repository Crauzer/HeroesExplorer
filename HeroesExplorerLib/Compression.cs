using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace HeroesExplorerLib
{
    public static class Compression
    {
        public static byte[] DecompressZLib(byte[] buffer)
        {
            using (MemoryStream memoryBuffer = new MemoryStream(buffer))
            {
                using (MemoryStream decompressedBuffer = new MemoryStream())
                {
                    using (DeflateStream deflateBuffer = new DeflateStream(memoryBuffer, CompressionMode.Decompress))
                    {
                        deflateBuffer.CopyTo(decompressedBuffer);
                    }
                    return decompressedBuffer.ToArray();
                }
            }
        }
    }
}
