using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWar2
{

    class Circle : IGameObject
    {

        public GraphicsDeviceManager Graphics { get; set; }

        public Vector2 Position { get; set; }

        public bool Expired { get; protected set; }

        public float Radius { get; set; }

        public Color LineColor { get; set; }

        public uint LineCount { get; set; }

        private VertexPositionColor[] vertices;

        public Circle(GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount)
        {

            Graphics = graphics;

            Position = position;

            Radius = radius;

            LineColor = lineColor;

            LineCount = lineCount;

            CreateVertices();

        }

        private void CreateVertices()
        {

            vertices = new VertexPositionColor[LineCount];

            for (int i = 0; i < LineCount - 1; i++)
            {
                float angle = (float)(i / (float)(LineCount - 1) * Math.PI * 2);

                vertices[i].Position = new Vector3(Position.X + (float)Math.Cos(angle) * Radius, Position.Y + (float)Math.Sin(angle) * Radius, 0);
                vertices[i].Color = LineColor;
            }
            vertices[LineCount - 1] = vertices[0];

        }

           

        protected void DrawLineStrip(VertexPositionColor[] array)
        {


            Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, array, 0, array.Length - 1);

        }

        public virtual void Draw()
        {
            CreateVertices();

            DrawLineStrip(vertices);

        }

        public void CentreToViewport()
        {
            Viewport viewport = Graphics.GraphicsDevice.Viewport;

            Position = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        }

    }
}
