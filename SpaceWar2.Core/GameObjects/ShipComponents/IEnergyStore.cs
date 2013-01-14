namespace DEMW.SpaceWar2.Core.GameObjects.ShipComponents
{
    public interface IEnergyStore
    {
        float Level { get; }
        float RequestEnergy(float amountRequested);
        void Recharge(float deltaT);
    }
}