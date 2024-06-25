using System;
using System.Collections.Generic;

using Microsoft.DirectX;

namespace LR4_AKG
{
    class Surface
    {
        public Vertex[] vertexNumeric; // Набор точек
        public Triangle[] triangleNumeric; // Набор треугольников
        public Dictionary<Vertex, Vertex> EdgesList = new Dictionary<Vertex, Vertex>(0);
        

        // Конструктор
        public Surface(Vertex one, Vertex two, Vertex tree, Vertex four)
        {
            this.vertexNumeric = new Vertex[4] { one, two, tree, four };
            this.triangleNumeric = new Triangle[2] { new Triangle(one, two, tree), new Triangle(one, four, tree) }; // Создаем два треугольника из которого и состоит плоскость4

            this.EdgesList.Add(one, two);
            this.EdgesList.Add(two, tree);
            this.EdgesList.Add(tree, four);
            this.EdgesList.Add(four, one);
        }

        // Получить точки
        public Vector3[] _getVertex()
        {
            List<Vector3> vec = new List<Vector3>(0);
            foreach (Vertex i in this.vertexNumeric)
            {
                vec.Add(i.Position);
            }
            return vec.ToArray();
        }

        // Получить среднюю точку плоскости
        public Vector3 _getCenterVertexSurface()
        {
            float x = (this.vertexNumeric[0].Position.X + this.vertexNumeric[1].Position.X + this.vertexNumeric[2].Position.X + this.vertexNumeric[3].Position.X) / 4,
                  y = (this.vertexNumeric[0].Position.Y + this.vertexNumeric[1].Position.Y + this.vertexNumeric[2].Position.Y + this.vertexNumeric[3].Position.Y) / 4,
                  z = (this.vertexNumeric[0].Position.Z + this.vertexNumeric[1].Position.Z + this.vertexNumeric[2].Position.Z + this.vertexNumeric[3].Position.Z) / 4;
            return new Vector3(x,y,z);
        }

        // Получить средние точки 4 граней
        public Vector3[] _getCenterEdge()
        {
            Vector3[] vertCenterEdge = new Vector3[4];
            vertCenterEdge[0] = new Vector3((vertexNumeric[0].Position.X + vertexNumeric[1].Position.X) / 2, 
                                           (vertexNumeric[0].Position.Y + vertexNumeric[1].Position.Y) / 2, 
                                           (vertexNumeric[0].Position.Z + vertexNumeric[1].Position.Z) / 2);

            vertCenterEdge[1] = new Vector3((vertexNumeric[1].Position.X + vertexNumeric[2].Position.X) / 2,
                                           (vertexNumeric[1].Position.Y + vertexNumeric[2].Position.Y) / 2,
                                           (vertexNumeric[1].Position.Z + vertexNumeric[2].Position.Z) / 2);

            vertCenterEdge[2] = new Vector3((vertexNumeric[2].Position.X + vertexNumeric[3].Position.X) / 2,
                                           (vertexNumeric[2].Position.Y + vertexNumeric[3].Position.Y) / 2,
                                           (vertexNumeric[2].Position.Z + vertexNumeric[3].Position.Z) / 2);

            vertCenterEdge[3] = new Vector3((vertexNumeric[3].Position.X + vertexNumeric[0].Position.X) / 2,
                                           (vertexNumeric[3].Position.Y + vertexNumeric[0].Position.Y) / 2,
                                           (vertexNumeric[3].Position.Z + vertexNumeric[0].Position.Z) / 2);
            return vertCenterEdge;
        }

        // Статический метод получения центра двух точек
        public static Vector3 ReturnEdgeCenter(Vector3 v1, Vector3 v2)
        {
            float x, y, z;
            x = (v1.X + v2.X) / 2;
            y = (v1.Y + v2.Y) / 2;
            z = (v1.Z + v2.Z) / 2;
            return new Vector3(x, y, z);
        }
    }
}
