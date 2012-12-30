using DEMW.SpaceWar2.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _target;
        private readonly Volume _volume;

        public Camera(Volume volume)
            : this(new Vector3(0, 0, 1), Vector3.Zero, volume)
        {
        }

        private Camera(Vector3 position, Vector3 target, Volume volume)
        {
            _position = position;
            _target = target;
            _volume = volume;

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
            _volume.Expand(amount);
            UpdateProjection();
        }

        private void UpdateProjection()
        {
            Projection = Matrix.CreateOrthographicOffCenter(
                _volume.MinX, _volume.MaxX,
                _volume.MaxY, _volume.MinY,
                _volume.MinZ, _volume.MaxZ
            );
        }
    }
}
