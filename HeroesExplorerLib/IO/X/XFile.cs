using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeroesExplorerLib.IO.X
{
    public class XFile
    {
        public List<XFlag> Flags { get; private set; } = new List<XFlag>();
        public Dictionary<string, byte[]> Files { get; private set; } = new Dictionary<string, byte[]>();
        public XDefiitions Definitions { get; private set; }
        public XGeometry Geometry { get; private set; } 
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

                UInt32 DefinitionsUncompressed = br.ReadUInt32();
                UInt32 GeometryUncompressed = br.ReadUInt32();
                UInt32 DefinitionsCompressed = br.ReadUInt32();
                UInt32 GeometryCompressed = br.ReadUInt32();

                byte[] DefinitionsBuffer = DecompressBuffer(br.ReadBytes((int)DefinitionsCompressed));
                byte[] GeometryBuffer = DecompressBuffer(br.ReadBytes((int)GeometryCompressed));

                this.Files.Add(Path.GetFileNameWithoutExtension(fileLocation) + ".def", DefinitionsBuffer);
                this.Files.Add(Path.GetFileNameWithoutExtension(fileLocation) + ".geo", GeometryBuffer);

                this.Definitions = new XDefiitions(new MemoryStream(DefinitionsBuffer));
                this.Geometry = new XGeometry(new MemoryStream(GeometryBuffer));
            }
        }

        public byte[] DecompressBuffer(byte[] buffer)
        {
            byte[] dataToDecompress = new byte[buffer.Length - 4];
            Buffer.BlockCopy(buffer, 2, dataToDecompress, 0, buffer.Length - 4);
            return Compression.DecompressZLib(dataToDecompress);
        }
        public void ExtractFiles(string location)
        {
            foreach (KeyValuePair<string, byte[]> Entry in this.Files)
            {
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }
                File.WriteAllBytes(location + "//" + Entry.Key, Entry.Value);
            }
        }
    }
}
