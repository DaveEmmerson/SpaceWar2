using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    internal class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }

        public Matrix View { get { return Matrix.CreateLookAt(Position, Target, Vector3.Up); } }
        public Matrix Projection { get; protected set; }

        public Camera(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
            
            //TODO MW figure out why the Y axis is flipped
            Projection = Matrix.CreateOrthographicOffCenter(
                -400f, 400f,
                240f, -240f,
                -1000.0f, 1000.0f
            );
        }

        public void Pan(Vector3 vector) 
        {
            Position += vector;
            Target += vector;
        }
    }
}
