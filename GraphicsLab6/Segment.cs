using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLab6
{
    class Segment
    {
        readonly Point a;
        readonly Point b;

        public Segment(Point a, Point b)
        {
            this.a = a;
            this.b = b;
        }

        public double Lenght()
        {
            return Math.Sqrt((a.X - b.X) ^ 2 + (a.Y - b.Y) ^ 2);
        }
    }

    class Segment3D
    {
        readonly Point3D a;
        readonly Point3D b;

        public Segment3D(Point3D a, Point3D b)
        {
            this.a = a;
            this.b = b;
        }

        public double Lenght()
        {
            return Math.Sqrt(Math.Pow((a.X - b.X),2) + Math.Pow((a.Y - b.Y),2) + Math.Pow((a.Z - b.Z),2));
        }
    }
}
