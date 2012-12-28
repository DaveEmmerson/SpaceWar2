using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    public class Arrow : IArrow
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly VertexPositionColor[] _vertices;

        public static IArrow CreateArrow(IGraphicsDeviceManager graphics, Vector2 position, Vector2 direction, Color color, float radius)
        {
            if (direction == Vector2.Zero)
            {
                //TODO Could use the null object pattern
                return null;
            }

            return new Arrow(graphics, position, direction, color, radius);
        }
        
        private Arrow(IGraphicsDeviceManager graphics, Vector2 position, Vector2 direction, Color color, float radius)
        {
            _graphics = graphics as GraphicsDeviceManager;
            _vertices = new VertexPositionColor[6];
            
            var length = 3f * (float)Math.Sqrt(direction.Length());

            direction.Normalize();

            var arrowHeadSize = radius / 3f;
            var perpendicular = new Vector3(-direction.Y, direction.X, 0);
            var arrowBase = new Vector3(position + (direction * radius), 0);
            var arrowHead = new Vector3(position + (direction * (radius + length)), 0);
            var arrowPoint = new Vector3(position + (direction * (radius + length + arrowHeadSize)), 0);
            
            _vertices[0].Position = arrowBase;
            _vertices[1].Position = arrowHead;
            _vertices[2].Position = arrowHead - (perpendicular * arrowHeadSize);
            _vertices[3].Position = arrowPoint;
            _vertices[4].Position = arrowHead + (perpendicular * arrowHeadSize);
            _vertices[5].Position = arrowHead;

            for (var i = 1; i <=5; i++)
            {
                _vertices[i].Color = color;
            }
            _vertices[0].Color = Color.Transparent;
        }

        public void Draw()
        {
            _graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices, 0, _vertices.Length - 1);
        }

        public VertexPositionColor[] Verticies
        {
            get { return _vertices; }
        }
    }
}
