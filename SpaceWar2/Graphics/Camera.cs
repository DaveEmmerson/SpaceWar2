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
        {
            _position = new Vector3(0, 0, 1);
            _target = Vector3.Zero;
            _volume = volume;

            UpdateView();
            UpdateProjection();
        }

        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        public void Pan(Vector3 vector) 
        {
            _position += vector;
            _target += vector;
            UpdateView();
        }

        public void Zoom(float amount)
        {
            _volume.Expand(amount);
            UpdateProjection();
        }

        private void UpdateView()
        {
            View = Matrix.CreateLookAt(_position, _target, Vector3.Up);
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
