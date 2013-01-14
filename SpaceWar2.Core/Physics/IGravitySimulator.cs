using DEMW.SpaceWar2.Core.GameObjects;

namespace DEMW.SpaceWar2.Core.Physics
{
    public interface IGravitySimulator
    {
        void RegisterSource(IGameObject source);
        void RegisterParticipant(IGameObject participant);
        void Unregister(IGameObject item);
        void Simulate();
    }
}