using HeroesExplorerLib.IO.X;

namespace HeroesExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            XFile xfile = new XFile("029.x");
            xfile.ExtractFiles("029");
        }
    }
}
