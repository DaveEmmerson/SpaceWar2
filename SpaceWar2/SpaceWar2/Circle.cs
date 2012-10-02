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

        public float Mass { get; set; }

        public float Radius { get; set; }

        public Vector2 Acceleration { get; set; }

        public Color LineColor { get; set; }

        public uint LineCount { get; set; }

        private VertexPositionColor[] _vertices;

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
            _vertices = new VertexPositionColor[LineCount];

            for (var i = 0; i < LineCount - 1; i++)
            {
                var angle = (float)(i / (float)(LineCount - 1) * Math.PI * 2);

                _vertices[i].Position = new Vector3((float)Math.Cos(angle) * Radius, (float)Math.Sin(angle) * Radius, 0);
                _vertices[i].Color = LineColor;
            }

            _vertices[LineCount - 1] = _vertices[0];
        }
        
        protected void DrawLineStrip(VertexPositionColor[] array)
        {
            Graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, array, 0, array.Length - 1);
        }

        public virtual void Draw()
        {

            DrawLineStrip(_vertices);
        }
    }
}
