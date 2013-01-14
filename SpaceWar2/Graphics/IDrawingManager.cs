using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Graphics
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