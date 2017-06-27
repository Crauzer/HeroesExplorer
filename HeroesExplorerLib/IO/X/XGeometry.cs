using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesExplorerLib.Helpers.Structures;

namespace HeroesExplorerLib.IO.X
{
    public class XGeometry
    {
        public List<Vector3> Vertices { get; private set; } = new List<Vector3>();
        public List<Vector3> Normals { get; private set; } = new List<Vector3>();
        public List<Vector2> UVs { get; private set; } = new List<Vector2>();
        public List<UInt16> Indices { get; private set; } = new List<UInt16>();
        public XGeometry(Stream s)
        {

        }
    }
}
