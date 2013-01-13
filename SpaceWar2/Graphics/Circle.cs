﻿using System;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    public class Circle
    {
        private VertexPositionColor[] _vertices;

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
            _vertices = new VertexPositionColor[lineCount];

            for (var i = 0; i < lineCount - 1; i++)
            {
                var angle = (float)(i / (float)(lineCount - 1) * Math.PI * 2);

                _vertices[i].Position = new Vector3((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius, 0);
                _vertices[i].Color = lineColor;
            }

            _vertices[lineCount - 1] = _vertices[0];
        }

        public void Draw(IGraphicsDevice graphicsDevice)
        {
            if (graphicsDevice != null)
            {
                graphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices, 0, _vertices.Length - 1);
            }
        }

        public VertexPositionColor[] Vertices()
        {
            return _vertices;
        }
    }
}
