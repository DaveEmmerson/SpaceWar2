using DEMW.SpaceWar2.GameObjects;

namespace DEMW.SpaceWar2.Physics
{
    public interface IGravitySimulator
    {
        void RegisterSource(IGameObject source);
        void RegisterParticipant(IGameObject participant);
        void Unregister(IGameObject item);
        void Simulate();
    }
}