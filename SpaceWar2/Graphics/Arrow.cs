using System;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    public class Arrow : IArrow
    {
        public static IArrow CreateArrow(Vector2 position, Vector2 direction, Color color, float radius)
        {
            if (direction == Vector2.Zero)
            {
                return new NullArrow();
            }

            return new Arrow(position, direction, color, radius);
        }

        private Arrow(Vector2 position, Vector2 direction, Color color, float radius)
        {
            Verticies = new VertexPositionColor[6];
            
            var length = 3f * (float)Math.Sqrt(direction.Length());

            direction.Normalize();

            var arrowHeadSize = radius / 3f;
            var perpendicular = new Vector3(-direction.Y, direction.X, 0);
            var arrowBase = new Vector3(position + (direction * radius), 0);
            var arrowHead = new Vector3(position + (direction * (radius + length)), 0);
            var arrowPoint = new Vector3(position + (direction * (radius + length + arrowHeadSize)), 0);
            
            Verticies[0].Position = arrowBase;
            Verticies[1].Position = arrowHead;
            Verticies[2].Position = arrowHead - (perpendicular * arrowHeadSize);
            Verticies[3].Position = arrowPoint;
            Verticies[4].Position = arrowHead + (perpendicular * arrowHeadSize);
            Verticies[5].Position = arrowHead;

            for (var i = 1; i <=5; i++)
            {
                Verticies[i].Color = color;
            }
            Verticies[0].Color = Color.Transparent;
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
