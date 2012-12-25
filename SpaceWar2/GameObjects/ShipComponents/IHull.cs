namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public interface IHull
    {
        float Level { get; }
        void Damage(float amount);
    }
}