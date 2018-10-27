﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLab6
{
    enum PolyhedronType
    {
        Tetrahedron,
        Hexahedron,
        Octahedron,
        Icosahedron,
        Dodecahedron
    };

    static class SchlafliSymbol
    {
        static public Dictionary<string, Tuple<int, int>> schlafliSymbol = new Dictionary<string, Tuple<int, int>>
        {
            ["Tetrahedron"] = Tuple.Create<int, int>(3, 3),
            ["Hexahedron"] = Tuple.Create<int, int>(3, 4),
            ["Octahedron"] = Tuple.Create<int, int>(4, 3),
            ["Icosahedron"] = Tuple.Create<int, int>(3, 5),
            ["Dodecahedron"] = Tuple.Create<int, int>(5, 3)
        };
    }
    

    class Polyhedron
    {
        public int CountVertex;
        public int CountSegment;
        public int CountEdge;
        public PolyhedronType Type;
        public PointF CentrePoint;
        public int SegmentLength;
        public List<Point3D> vertexes;

        public Polyhedron(PolyhedronType type,int len)
        {
            var ss = SchlafliSymbol.schlafliSymbol[type.ToString()];
            CountVertex = 4 * ss.Item1 / (4 - (ss.Item1 - 2) * (ss.Item2 - 2));
            CountSegment = ss.Item2 * CountVertex / 2;
            CountEdge = ss.Item2 * CountVertex / ss.Item1;
            Type = type;
           //CentrePoint = centrePoint;
            SegmentLength = len;
            vertexes = new List<Point3D>();
            if (type.ToString() == "Tetrahedron")
            {
                vertexes.Add(new Point3D(0, 0, 0));
                vertexes.Add(new Point3D(len, 0, 0));
                vertexes.Add(new Point3D(len/2, len * Math.Sqrt(3)/2, 0)); 
                vertexes.Add(new Point3D(len/2, len * Math.Sqrt(3)/6, len * Math.Sqrt(6)/3));
            }
        }

        public List<PointF> DrawPolyhedron(Size size)
        {
            var res = new List<PointF>();
            var x = size.Width / 2 - SegmentLength/2;
            var y = size.Height / 2 - SegmentLength/2;
            var z = 1.0;
            if (Type.ToString() == "Tetrahedron")
            {
                foreach (var item in vertexes)
                {
                    if (item.Z != 0)
                        z = item.Z;
                    res.Add(new PointF((float)(item.X / z + x), (float)(item.Y / z) + y));
                }
            }
            return res;
        }
    }
}