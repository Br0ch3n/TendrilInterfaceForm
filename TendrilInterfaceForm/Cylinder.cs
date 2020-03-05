using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;
using Tao.FreeGlut;

namespace TendrilInterfaceForm
{
    class Cylinder
    {
        public VBO<Vector3> vertices, vertColors;
        public VBO<int> vertElements;
        private float Radius, Height;
        private int Slices, Stacks, NumVertices;
        private Vector3 Origin, Color;

        public Cylinder(Vector3 origin, float height, float radius, int slices, int stacks, Vector3 color)
        {
            Origin = origin;
            Height = height;
            Radius = radius;
            Slices = slices;
            Stacks = stacks;
            Color = color;
            FormCylinder();
        }

        private void FormCylinder()
        {
            NumVertices = Stacks * Slices;
            PlaceVertices();
        }

        public void DisposeCylinder()
        {
            vertices.Dispose();
            vertColors.Dispose();
            vertElements.Dispose();
        }

        private void PlaceVertices()
        {
            float angle = 360.0f / Slices;
            float step = Height / Stacks;
            Vector3[] tempVect = new Vector3[NumVertices];
            int[] tempInt = new int[NumVertices];
            int ndx = 0;
            for (float y = Origin.Y; y < (Stacks * step); y += step)
            {
                for(float theta = 0; theta < 360; theta += angle)
                {
                    tempVect[ndx] = new Vector3(Radius * (float)Math.Cos(theta * Math.PI / 180), y, Radius * (float)Math.Sin(theta * Math.PI / 180));
                    //Console.WriteLine(theta.ToString() + tempVect[ndx].ToString());
                    ndx++;
                }
            }
            vertices = new VBO<Vector3>(tempVect);

            for (ndx = 0; ndx < NumVertices; ndx++)
            {
                tempVect[ndx] = Color;
                tempInt[ndx] = ndx;
            }
            vertColors = new VBO<Vector3>(tempVect);
            vertElements = new VBO<int>(tempInt, BufferTarget.ElementArrayBuffer);
        }
    }
}
