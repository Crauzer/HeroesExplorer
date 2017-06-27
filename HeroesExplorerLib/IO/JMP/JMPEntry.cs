using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HeroesExplorerLib.IO.JMP
{
    [DebuggerDisplay("{Name}")]
    public class JMPEntry
    {
        public string Name { get; private set; }
        public byte[] Data { get; private set; }
        public JMPEntry(BinaryReader br)
        {
            this.Name = Encoding.ASCII.GetString(br.ReadBytes(260)).Replace("\0", "").Remove(0, 3);
            UInt32 DataOffset = br.ReadUInt32();
            UInt32 CompressedSize = br.ReadUInt32();
            UInt32 UncompressedSize = br.ReadUInt32();
            br.BaseStream.Seek(32, SeekOrigin.Current);

            long ReturnPosition = br.BaseStream.Position;
            br.BaseStream.Seek(DataOffset, SeekOrigin.Begin);
            this.Data = br.ReadBytes((int)CompressedSize);
            br.BaseStream.Seek(ReturnPosition, SeekOrigin.Begin);
        }
        public byte[] GetDecompressedData()
        {
            byte[] dataToDecompress = new byte[this.Data.Length - 4];
            Buffer.BlockCopy(this.Data, 2, dataToDecompress, 0, this.Data.Length - 4);
            return Compression.DecompressZLib(dataToDecompress);
        }
    }
}
