namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public interface IShield
    {
        float Level { get; }
        void Recharge(float deltaT);
        float Damage(float amount);
    }
}