using DEMW.SpaceWar2.Core.GameObjects;

namespace DEMW.SpaceWar2.GameObjects
{
    public interface IShip : IGameObject
    {
        float AngularVelocity { get; set; }
        float RequestEnergy(float amountRequested);

        string DebugDetails { get; }
    }
}