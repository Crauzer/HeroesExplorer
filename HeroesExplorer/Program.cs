using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesExplorerLib.JMP;
using HeroesExplorerLib.X;

namespace HeroesExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            XFile xfile = new XFile("jiutoulong.x");
            xfile.ExtractFiles("xfile");
        }
    }
}
