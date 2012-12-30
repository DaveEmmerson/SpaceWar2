using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Physics
{
    public interface IUniverse
    {
        Volume Volume { get; }
        
        void Register(IGameObject managedObject);
        void UnRegister(IGameObject managedObject);
        void Update();
    }
}