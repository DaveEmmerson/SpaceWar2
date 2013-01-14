using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Physics;

namespace DEMW.SpaceWar2.Core.Graphics
{
    public interface IDrawingManager
    {
        Camera ActiveCamera { get; }
        void Register(IGameObject gameObject);
        void Unregister(IGameObject gameObject);
        void DrawGameObjects();
        void ResetCamera(IUniverse universe);
    }
}