using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    class Circle
    {
        private readonly GraphicsDeviceManager _graphics;
        private VertexPositionColor[] _vertices;

        public Circle(IGraphicsDeviceManager graphics, float radius, Color lineColor, uint lineCount)
        {
            _graphics = graphics as GraphicsDeviceManager;
            CreateVertices(radius, lineColor, lineCount);
        }

        private void CreateVertices(float radius, Color lineColor, uint lineCount)
        {
            _vertices = new VertexPositionColor[lineCount];

            for (var i = 0; i < lineCount - 1; i++)
            {
                var angle = (float)(i / (float)(lineCount - 1) * Math.PI * 2);

                _vertices[i].Position = new Vector3((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius, 0);
                _vertices[i].Color = lineColor;
            }

            _vertices[lineCount - 1] = _vertices[0];
        }
        
        public void Draw()
        {
            _graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices, 0, _vertices.Length - 1);
        }
    }
}
