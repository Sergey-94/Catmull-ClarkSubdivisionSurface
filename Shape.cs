using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace LR4_AKG
{
    class Shape
    {
        public List<Surface> surfaceNumeric = new List<Surface>(0); // Набор плоскостей
        public Vector3 Center; // Центр фигуры

        // Конструктор
        public Shape( Vector3 center)
        {
            this.Center = center;
        }

        // Добавить поверхность
        public void _AddSurface(Surface surface)
        {
            this.surfaceNumeric.Add(surface);
        }

        // Вернуть массив прилежащих граней
        public Surface[] GetSurface(Vector3 v1)
        {
            List<Surface> surface = new List<Surface>(0);
            foreach (Surface i in surfaceNumeric)
            {
                foreach (Vector3 j in i._getVertex())
                {
                    if (j == v1) { surface.Add(i); }
                }
            }
            return surface.ToArray();
        }

        // Найти барицентр центров прилежащих граней
        public Vector3 BaricenterSurfases(Vector3 vertex)
        {
            Surface[] surface = GetSurface(vertex);

            float x = 0, y = 0, z = 0;
            for (int i = 0; i < surface.Length; i++)
            {
                Vector3 tempVec = surface[i]._getCenterVertexSurface();
                x += tempVec.X;
                y += tempVec.Y;
                z += tempVec.Z;
            }
            x = x / surface.Length;
            y = y / surface.Length;
            z = z / surface.Length;

            return new Vector3(x, y, z);
        }

        // Найти барицентр центров прилежащих ребер
        public Vector3 BaricenterCenterEdges(Vector3 vertex)
        {
            Surface[] surface = GetSurface(vertex);

            List<Vector3> EdgesVertexs = new List<Vector3>(0);

            foreach (Surface s in surface)
            {
                foreach (KeyValuePair<Vertex,Vertex> g in s.EdgesList)
                {
                    if (g.Key.Position == vertex)
                    {
                        if (!EdgesVertexs.Contains(g.Value.Position))
                        {
                            EdgesVertexs.Add(g.Value.Position);
                        }
                                              
                    }
                    if (g.Value.Position == vertex)
                    {
                        if (!EdgesVertexs.Contains(g.Key.Position))
                        {
                            EdgesVertexs.Add(g.Key.Position);
                        }                          
                    }
                }
            }

            float x = 0, y = 0, z = 0;
            foreach (Vector3 i in EdgesVertexs)
            {
                Vector3 tmp = Surface.ReturnEdgeCenter(vertex, i);
                x += tmp.X;
                y += tmp.Y;
                z += tmp.Z;
            }

            x = x / EdgesVertexs.Count;
            y = y / EdgesVertexs.Count;
            z = z / EdgesVertexs.Count;

            return new Vector3(x, y, z);
        }

        // Разделить грань
        public Surface[] SubdevideSurface(Surface surface)
        {
            Surface[] subSurface = new Surface[4];

            Vector3 CenterSurface = surface._getCenterVertexSurface(); // Переменная с координатами центра поверхности
            Vector3[] EdgesCenter = surface._getCenterEdge(); // Центры ребер
            Vector3[] AngleVertex = surface._getVertex(); // Угловые точки плоскости

            for (int i = 0; i < AngleVertex.Length; i++)
            {
                Vector3 F = BaricenterSurfases(AngleVertex[i]);
                Vector3 R = BaricenterCenterEdges(AngleVertex[i]);

                float n = GetSurface(AngleVertex[i]).Length;
                float m1 = (n - 3) / n;
                float m2 = 1 / n;
                float m3 = 2 / n;

                AngleVertex[i] = ((m2 * F) + (m3 * R) + (m1 * AngleVertex[i]));
            }

            if (this.surfaceNumeric.Count == 6) { for (int i = 0; i < EdgesCenter.Length; i++) { EdgesCenter[i] = EdgesCenter[i] * 0.7f; } }
            if (this.surfaceNumeric.Count == 24) { for (int i = 0; i < EdgesCenter.Length; i++) { EdgesCenter[i] = EdgesCenter[i] * 0.94f; } }

            subSurface[0] = new Surface(new Vertex(AngleVertex[0], Color.White), new Vertex(EdgesCenter[0], Color.Green), 
                            new Vertex(CenterSurface,  Color.Yellow), new Vertex(EdgesCenter[3], Color.Green));

            subSurface[1] = new Surface(new Vertex(AngleVertex[1], Color.White), new Vertex(EdgesCenter[1], Color.Green), 
                            new Vertex(CenterSurface, Color.Yellow), new Vertex(EdgesCenter[0], Color.Green));

            subSurface[2] = new Surface(new Vertex(AngleVertex[2], Color.White), new Vertex(EdgesCenter[1], Color.Green), 
                            new Vertex(CenterSurface, Color.Yellow), new Vertex(EdgesCenter[2], Color.Green));

            subSurface[3] = new Surface(new Vertex(AngleVertex[3], Color.White), new Vertex(EdgesCenter[2], Color.Green), 
                            new Vertex(CenterSurface, Color.Yellow), new Vertex(EdgesCenter[3], Color.Green));

            return subSurface;
        }

        // Разделить все грани
        public void SubdivideShape()
        {
            if(this.surfaceNumeric.Count < 6144)
            {
                List<Surface> sur = new List<Surface>(0);

                foreach (Surface i in this.surfaceNumeric)
                {
                    foreach (Surface j in this.SubdevideSurface(i))
                    {
                        sur.Add(j);
                    }
                }
                this.surfaceNumeric = sur;
            }
        }
    }
}
