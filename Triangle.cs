using System;

namespace LR4_AKG
{
    class Triangle
    {
        public Vertex[] vertexNumeric; // Набор точек

        // Конструктор
        public Triangle(Vertex one, Vertex two, Vertex tree)
        {
            this.vertexNumeric = new Vertex[3] { one, two, tree };
        }
    }
}
