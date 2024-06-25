using Microsoft.DirectX;
using System.Drawing;

namespace LR4_AKG
{
    class Vertex
    {
        public Vector3 Position; // Позиця точки
        public int Color; // Цвет точки по умолчанию

        // Конструктор по вектору3 и цвету
        public Vertex(Vector3 XYZ, Color color)
        {
            this.Position = XYZ;
            this.Color = color.ToArgb();
        }

        // Конструктор по координатам
        public Vertex( float X, float Y, float Z)
        {
            this.Position = new Vector3(X, Y, Z);
            this.Color = System.Drawing.Color.Black.ToArgb();
        }

        // Конструктор с установкой цвета
        public Vertex(float X, float Y, float Z, int color)
        {
            this.Position = new Vector3(X, Y, Z);
            this.Color = color;
        }
    }
}
