using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _target;
        private readonly IUniverse _universe;

        internal static Camera GetDefault(IUniverse universe)
        {
            return new Camera(new Vector3(0, 0, 1), Vector3.Zero, universe);
        }

        private Camera(Vector3 position, Vector3 target, IUniverse universe)
        {
            _position = position;
            _target = target;
            _universe = universe;

            UpdateProjection();
        }

        public Matrix View { get { return Matrix.CreateLookAt(_position, _target, Vector3.Up); } }
        public Matrix Projection { get; private set; }

        public void Pan(Vector3 vector) 
        {
            _position += vector;
            _target += vector;
        }

        public void Zoom(float amount)
        {
            _universe.Expand(amount);
            UpdateProjection();
        }

        private void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographicOffCenter(
                _universe.MinX, _universe.MaxX,
                _universe.MaxY, _universe.MinY,
                _universe.MinZ, _universe.MaxZ
            );
        }
    }
}
