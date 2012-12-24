using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Physics
{
    public interface IUniverse
    {
        IUniverse CopyDimensions();
        float MinX { get; }
        float MaxX { get; }
        float MinY { get; }
        float MaxY { get; }
        float MinZ { get; }
        float MaxZ { get; }
        float Width { get; }
        float Height { get; }
        void Expand(float verticalAmount);
        void Contract(float verticalAmount);
        void Register(IGameObject managedObject);
        void UnRegister(IGameObject managedObject);
        void Update();
    }
}