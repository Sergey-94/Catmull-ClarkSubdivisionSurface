using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace LR4_AKG
{
    public partial class Form1 : Form
    {
        private Device device = null; // Девайс
        private VertexBuffer VB = null; // Вертексный буфер
        private CustomVertex.PositionColored[] verts = null; // Массив точек
        private List<Shape> ShapesShowShape = new List<Shape>(0);

        float angleX = 0.0f; // Переменная поворота по X
        float angleY = 0.0f; // Переменная поворота по Y
        float angleStepX = 0.01f; // Переменная поворота по X
        float angleStepY = 0.015f; // Переменная поворота по Y

        private Microsoft.DirectX.Direct3D.Font font = null; // Шрифт

        // Инициализируем панель
        public Form1()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            InitializeComponent();
            InitialGraphics();
            font = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font("Areal", 10.0f, FontStyle.Regular));  // Определяем шрифт
        }

        // Инициализируем графику
        private void InitialGraphics()
        {
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;

            pp.EnableAutoDepthStencil = true;
            pp.AutoDepthStencilFormat = DepthFormat.D16;

            device = new Device(0, DeviceType.Hardware, this.panel1, CreateFlags.HardwareVertexProcessing, pp); // Создаем девайс

            VB = new VertexBuffer(typeof(CustomVertex.PositionColored), 36, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            VB.Created += new EventHandler(this.OnVertexBufferCreate);
            OnVertexBufferCreate(VB, null);

            VB.SetData(verts, 0, LockFlags.None);
        }

        // Заполняем вертексный буфер
        private void OnVertexBufferCreate(object sender, EventArgs e)
        {
            VertexBuffer buffer = (VertexBuffer)sender;

            verts = new CustomVertex.PositionColored[0];
            buffer.SetData(verts, 0, LockFlags.None);
        }

        // Инициализация камеры
        private void SetupCamera()
        {
            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, this.Width / this.Height, 1.0f, 100.0f); // Установить настройки проекции
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, 16), new Vector3(), new Vector3(0, 1, 0)); // Установить настройки позиции камеры   

            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.None;
        }

        // Глобальный метод перерисовки
        private void Form1Form_Paint(object sender, PaintEventArgs e)
        {
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.CornflowerBlue, 1, 0); // Очищаем область рисования, Z-Буффер

            SetupCamera(); // Вызываем метод инициализации камеры

            device.BeginScene(); // Запускаем отрисовку сцены

            device.VertexFormat = CustomVertex.PositionColored.Format; // Вызываем метод вывода вершин
            device.SetStreamSource(0, VB, 0); // Устанавливаем буфер вершин

            if (ShapesShowShape.Count > 0)
            {
                DrawShapes(ShapesShowShape[0].Center.X, ShapesShowShape[0].Center.Y, ShapesShowShape[0].Center.Z); // Вызываем метод перерисовки объекта
                font.DrawText(null, string.Format(ShapesShowShape[0].surfaceNumeric.Count.ToString() + " Faces" + "\n"), new Rectangle(10, 10, 0, 0), DrawTextFormat.NoClip, Color.BlanchedAlmond);
            }

            device.EndScene(); // Конец отрисовки сцены
            device.Present(); // Показать сцену
            Update(); // Обновляем элементы интерфейса
            this.Invalidate(); // Перерисовать интерфейс
        }

        // Метод перерисовки модели
        private void DrawShapes(float x, float y, float z)
        {
            angleX += angleStepX;
            angleY += angleStepY;
            device.Transform.World = Matrix.RotationAxis(new Vector3(1, 0, 0), angleX) * (Matrix.Translation(x, y, z)) * Matrix.RotationAxis(new Vector3(0, 1, 0), angleY);

            int countTriangle = 1;
            foreach (Shape i in ShapesShowShape)
            {
                foreach (Surface j in i.surfaceNumeric)
                {
                    countTriangle += j.triangleNumeric.Length;
                }
            }
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, verts.Length/3);
        }
        
        // Показать / скрыть сетку
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                device.RenderState.FillMode = FillMode.WireFrame;
            }
            else
            {
                device.RenderState.FillMode = FillMode.Solid;
            }
        }
        // Вращать фигуру
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                this.angleStepX = 0; this.angleStepY = 0;
            }
            else
            {
                this.angleStepX = 0.01f; this.angleStepY = 0.015f;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Shape shape = new Shape(new Vector3(0, 0, 0));
            // Left
            shape._AddSurface(
                new Surface(new Vertex(-2, 2, 2, Color.Blue.ToArgb()), new Vertex(-2, 2, -2, Color.Green.ToArgb()), 
                new Vertex(-2, -2, -2, Color.Green.ToArgb()), new Vertex(-2, -2, 2, Color.Green.ToArgb())));

            // Up
            shape._AddSurface(
                new Surface(new Vertex(-2, 2, 2, Color.Blue.ToArgb()), new Vertex(-2, 2, -2, Color.Green.ToArgb()), 
                new Vertex(2, 2, -2, Color.Green.ToArgb()), new Vertex(2, 2, 2, Color.Green.ToArgb())));

            // Bottom
            shape._AddSurface(
                new Surface(new Vertex(-2, -2, 2, Color.Blue.ToArgb()), new Vertex(-2, -2, -2, Color.Green.ToArgb()), 
                new Vertex(2, -2, -2, Color.Green.ToArgb()), new Vertex(2, -2, 2, Color.Green.ToArgb())));

            // Right
            shape._AddSurface(
                new Surface(new Vertex(2, 2, 2, Color.Blue.ToArgb()), new Vertex(2, 2, -2, Color.Green.ToArgb()), 
                new Vertex(2, -2, -2, Color.Green.ToArgb()), new Vertex(2, -2, 2, Color.Green.ToArgb())));

            // Front
            shape._AddSurface(
                new Surface(new Vertex(2, 2, 2, Color.Blue.ToArgb()), new Vertex(-2, 2, 2, Color.Green.ToArgb()), 
                new Vertex(-2, -2, 2, Color.Green.ToArgb()), new Vertex(2, -2, 2, Color.Green.ToArgb())));

            // Back
            shape._AddSurface(
                new Surface(new Vertex(2, 2, -2, Color.Blue.ToArgb()), new Vertex(-2, 2, -2, Color.Green.ToArgb()), 
                new Vertex(-2, -2, -2, Color.Green.ToArgb()), new Vertex(2, -2, -2, Color.Green.ToArgb())));
            
            if (ShapesShowShape.Count == 0)
            {
                this.ShapesShowShape.Add(shape);
            }
            else
            {
                this.ShapesShowShape[0] = shape;
            }

            // Обходим все точки всех объектов
            List<Vertex> TempVertexMassive = new List<Vertex>(0);

            foreach (Shape i in ShapesShowShape)
            {
                foreach (Surface j in i.surfaceNumeric)
                {
                    foreach (Triangle k in j.triangleNumeric)
                    {
                        foreach (Vertex l in k.vertexNumeric)
                        {
                            TempVertexMassive.Add(l);
                        }
                    }
                }
            }
            AddVertexBuffer_NewRange(TempVertexMassive.ToArray());
        }

        private void AddVertexBuffer_AddRange(Vertex[] vertices)
        {
            CustomVertex.PositionColored[] ver = new CustomVertex.PositionColored[verts.Length + vertices.Length];
            for (int i = 0; i < verts.Length; i++)
            {
                ver[i] = verts[i];
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                ver[i + verts.Length] = new CustomVertex.PositionColored(vertices[i].Position, vertices[i].Color);
            }

            verts = ver;
            VB = new VertexBuffer(typeof(CustomVertex.PositionColored), verts.Length, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            VertexBuffer buffer = VB;
            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void AddVertexBuffer_NewRange(Vertex[] vertices)
        {
            CustomVertex.PositionColored[] ver = new CustomVertex.PositionColored[vertices.Length];
            
            for (int i = 0; i < vertices.Length; i++)
            {
                ver[i] = new CustomVertex.PositionColored(vertices[i].Position, vertices[i].Color);
            }

            verts = ver;
            VB = new VertexBuffer(typeof(CustomVertex.PositionColored), verts.Length, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            VertexBuffer buffer = VB;
            buffer.SetData(verts, 0, LockFlags.None);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ShapesShowShape[0].SubdivideShape();

            List<Vertex> TempVertexMassive = new List<Vertex>(0);

            foreach (Shape i in ShapesShowShape)
            {
                foreach (Surface j in i.surfaceNumeric)
                {
                    foreach (Triangle k in j.triangleNumeric)
                    {
                        foreach (Vertex l in k.vertexNumeric)
                        {
                            TempVertexMassive.Add(l);
                        }
                    }
                }
            }
            AddVertexBuffer_NewRange(TempVertexMassive.ToArray());
        }
    }
}
