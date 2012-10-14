using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    internal class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Universe Universe { get; set; }

        public Matrix View { get { return Matrix.CreateLookAt(Position, Target, Vector3.Up); } }
        public Matrix Projection { get; protected set; }

        public Camera(Vector3 position, Vector3 target, Universe universe)
        {
            Position = position;
            Target = target;
            
            Projection = Matrix.CreateOrthographicOffCenter(
                universe.MinX, universe.MaxX,
                universe.MaxY, universe.MinY,
                universe.MinZ, universe.MaxZ
            );
        }

        public void Pan(Vector3 vector) 
        {
            Position += vector;
            Target += vector;
        }

        internal static Camera GetDefault()
        {
            return new Camera(new Vector3(0, 0, 1), Vector3.Zero, Universe.GetDefault());
        }
    }
}
