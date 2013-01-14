using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Core.Graphics
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _target;
        private readonly Volume _volume;

        internal Camera(Volume volume)
        {
            _position = new Vector3(0, 0, 1);
            _target = Vector3.Zero;
            _volume = volume;

            UpdateView();
            UpdateProjection();
        }

        internal Matrix View { get; private set; }
        internal Matrix Projection { get; private set; }

        internal void Pan(Vector3 vector) 
        {
            _position += vector;
            _target += vector;
            UpdateView();
        }

        internal void Zoom(float amount)
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
