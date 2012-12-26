using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;

namespace DEMW.SpaceWar2.Graphics
{
    public interface IDrawingManager
    {
        Camera ActiveCamera { get; }
        void Register(IGameObject gameObject);
        void UnRegister(IGameObject gameObject);
        void DrawGameObjects();
        void ResetCamera(IUniverse universe);
    }
}