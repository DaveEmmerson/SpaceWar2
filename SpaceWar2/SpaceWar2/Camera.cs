using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2
{
    internal class Camera
    {

        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public Matrix View { get { return Matrix.CreateLookAt(Position, Target, Vector3.Up); } }
        public Matrix Projection { get; protected set; }

        public Camera(Vector3 position, Vector3 target, Viewport viewport)
        {

            Position = position;
            Target = target;

            Projection = Matrix.CreateOrthographicOffCenter(
                viewport.X, viewport.X + viewport.Width,
                viewport.Y + viewport.Height, viewport.Y,
                -1000.0f, 1000.0f
            );

        }

        public void Pan(Vector3 vector) {

            Position += vector;
            Target += vector;

        }

    }
}
