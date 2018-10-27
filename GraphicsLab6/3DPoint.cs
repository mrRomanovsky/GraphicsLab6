using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsLab6
{
    class Point3D
    {
        public double X;
        public double Y;
        public double Z;

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
    }
}
