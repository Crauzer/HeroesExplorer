using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeroesExplorerLib.IO.JMP
{
    public class JMPFile
    {
        public string Version { get; private set; }
        public List<JMPEntry> Files { get; private set; } = new List<JMPEntry>();
        public JMPFile(string fileLocation)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileLocation)))
            {
                string Magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (Magic != "DATA")
                    throw new Exception("Not a valid JMP file");

                this.Version = Encoding.ASCII.GetString(br.ReadBytes(3));
                br.BaseStream.Seek(50, SeekOrigin.Begin);

                UInt32 FileCount = br.ReadUInt32();
                for(int i = 0; i < FileCount; i++)
                {
                    this.Files.Add(new JMPEntry(br));
                }
            }
        }

        public void ExtractFiles(string location)
        {
            foreach(JMPEntry Entry in this.Files)
            {
                string DirectoryName = Path.GetDirectoryName(Entry.Name);
                if (!Directory.Exists(location + DirectoryName))
                {
                    Directory.CreateDirectory(location + DirectoryName);
                }
                File.WriteAllBytes(location + Entry.Name, Entry.GetDecompressedData());
            }
        }
    }
}
