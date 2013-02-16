using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Physics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Core.Graphics
{
    public interface IDrawingManager
    {
        void Register(IGameObject gameObject);
        void Unregister(IGameObject gameObject);
        void DrawGameObjects();

        Matrix CameraView { get; }
        Matrix CameraProjection { get; }

        void ResetCamera(IUniverse universe);
        void MoveCamera(Vector3 vector);
        void ZoomCamera(float amount);
    }
}