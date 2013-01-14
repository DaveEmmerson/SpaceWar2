using DEMW.SpaceWar2.Core.GameObjects;

namespace DEMW.SpaceWar2.Core.Physics
{
    public interface IUniverse
    {
        Volume Volume { get; }
        
        void Register(IGameObject managedObject);
        void Unregister(IGameObject managedObject);
        void Update();
    }
}