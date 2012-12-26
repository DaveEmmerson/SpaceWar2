using DEMW.SpaceWar2.GameObjects.ShipComponents;

namespace DEMW.SpaceWar2.GameObjects
{
    public interface IShipComponentFactory
    {
        IEnergyStore CreateEnergyStore();
        IShield CreateShield(IShip ship);
        IHull CreateHull(IGameObject ship);
        IThrusterArray CreateThrusterArray(IShip ship);
    }
}