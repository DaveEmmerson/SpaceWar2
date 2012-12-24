using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Graphics
{
    public interface IDrawingManager
    {
        Camera ActiveCamera { get; set; }
        void Register(IGameObject gameObject);
        void UnRegister(IGameObject gameObject);
        void DrawGameObjects();
    }
}