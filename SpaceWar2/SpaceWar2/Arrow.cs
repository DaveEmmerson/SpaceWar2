using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWar2
{
    class Arrow
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly VertexPositionColor[] _vertices;

        internal Arrow(GraphicsDeviceManager graphics, Vector2 vector, Color color, float radius)
        {
            _vertices = new VertexPositionColor[5];
            _graphics = graphics;

            if (vector == Vector2.Zero)
            {
                return;
            }

            vector.Normalize();

            var perpendicular = new Vector2(-vector.Y, vector.X);

            float arrowSize = radius / 4f;

            _vertices[0].Position = new Vector3(vector * radius, 0);
            _vertices[1].Position = new Vector3(vector * radius * 2, 0);
            _vertices[2].Position = new Vector3(vector * (radius * 2 - arrowSize) - perpendicular * arrowSize, 0);
            _vertices[3].Position = new Vector3(vector * (radius * 2 - arrowSize) - -perpendicular * arrowSize, 0);
            _vertices[4].Position = new Vector3(vector * radius * 2, 0);

            _vertices[0].Color = color;
            _vertices[1].Color = color;
            _vertices[2].Color = color;
            _vertices[3].Color = color;
            _vertices[4].Color = color;
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
