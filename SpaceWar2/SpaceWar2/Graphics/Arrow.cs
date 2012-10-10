using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    class Arrow
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly VertexPositionColor[] _vertices;

        internal Arrow(GraphicsDeviceManager graphics, Vector2 vector, Color color, float radius, Vector2 position)
        {
            _graphics = graphics;
            _vertices = new VertexPositionColor[6];
            
            if (vector == Vector2.Zero)
            {
                return;
            }
            
            var length = 3f * (float)Math.Sqrt(vector.Length());

            vector.Normalize();

            var arrowHeadSize = radius / 3f;
            var perpendicular = new Vector3(-vector.Y, vector.X, 0);
            var arrowBase = new Vector3(position + (vector * radius), 0);
            var arrowHead = new Vector3(position + (vector * (radius + length)), 0);
            var arrowPoint = new Vector3(position + (vector * (radius + length + arrowHeadSize)), 0);
            
            _vertices[0].Position = arrowBase;
            _vertices[1].Position = arrowHead;
            _vertices[2].Position = arrowHead - (perpendicular * arrowHeadSize);
            _vertices[3].Position = arrowPoint;
            _vertices[4].Position = arrowHead + (perpendicular * arrowHeadSize);
            _vertices[5].Position = arrowHead;

            for (var i = 0; i <=5; i++)
            {
                _vertices[i].Color = color;
            }
        }

        public virtual void Draw()
        {
            DrawLineStrip(_vertices);
        }

        internal void DrawLineStrip(VertexPositionColor[] array)
        {
            _graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, array, 0, array.Length - 1);
        }
    }
}
