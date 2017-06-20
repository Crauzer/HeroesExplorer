using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeroesExplorerLib.X
{
    public class XFile
    {
        public List<XFlag> Flags { get; private set; } = new List<XFlag>();
        public List<byte[]> Files { get; private set; } = new List<byte[]>();
        public XFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(5));
                if (Magic != "JUMPX")
                    throw new Exception("Not a valid X File");

                br.BaseStream.Seek(84, SeekOrigin.Begin);
                UInt32 FlagChunkSize = br.ReadUInt32();
                for(int i = 0; i < FlagChunkSize / 12; i++)
                {
                    this.Flags.Add(new XFlag(br));
                }

                UInt32 File1Uncompressed = br.ReadUInt32();
                UInt32 File2Uncompressed = br.ReadUInt32();
                UInt32 File1Compressed = br.ReadUInt32();
                UInt32 File2Compressed = br.ReadUInt32();

                this.Files.Add(br.ReadBytes((int)File1Compressed));
                this.Files.Add(br.ReadBytes((int)File2Compressed));
            }
        }

        public IEnumerable<byte[]> GetUncompressedFiles()
        {
            foreach(byte[] Entry in this.Files)
            {
                byte[] dataToDecompress = new byte[Entry.Length - 4];
                Buffer.BlockCopy(Entry, 2, dataToDecompress, 0, Entry.Length - 4);
                yield return Compression.DecompressZLib(dataToDecompress);
            }
        }
        public void ExtractFiles(string location)
        {
            int i = 0;
            foreach (byte[] Entry in GetUncompressedFiles())
            {
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                File.WriteAllBytes(location + "//" + i + "." + "file" + i, Entry);
                i++;
            }
        }
    }
}
