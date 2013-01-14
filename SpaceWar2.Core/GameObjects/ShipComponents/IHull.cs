namespace DEMW.SpaceWar2.Core.GameObjects.ShipComponents
{
    public interface IHull
    {
        float Level { get; }
        void Damage(float amount);
    }
}