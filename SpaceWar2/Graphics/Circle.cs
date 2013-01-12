using System;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    public class Circle
    {
        public Circle(float radius, Color lineColor, int lineCount)
        {
            const int minumumlineCount = 2;
            if (lineCount <= minumumlineCount)
            {
                throw new ArgumentException("must be at least " + minumumlineCount, "lineCount");
            }

            if (radius <= 0)
            {
                throw new ArgumentException("must be greater than 0", "radius");
            }
            
            CreateVertices(radius, lineColor, lineCount);
        }

        private void CreateVertices(float radius, Color lineColor, int lineCount)
        {
            Verticies = new VertexPositionColor[lineCount];

            for (var i = 0; i < lineCount - 1; i++)
            {
                var angle = (float)(i / (float)(lineCount - 1) * Math.PI * 2);

                Verticies[i].Position = new Vector3((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius, 0);
                Verticies[i].Color = lineColor;
            }

            Verticies[lineCount - 1] = Verticies[0];
        }

        public void Draw(IGraphicsDevice graphicsDevice)
        {
            if (graphicsDevice != null)
            {
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, Verticies, 0, Verticies.Length - 1);
            }
        }

        public VertexPositionColor[] Verticies { get; private set; }
    }
}
