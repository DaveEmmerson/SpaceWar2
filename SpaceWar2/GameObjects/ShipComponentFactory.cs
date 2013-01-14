using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.GameObjects.ShipComponents;

namespace DEMW.SpaceWar2.GameObjects
{
    internal class ShipComponentFactory : IShipComponentFactory
    {
        //Todo consider parameterizing these methods rather than just returning defaults
        public IEnergyStore CreateEnergyStore()
        {
            return new EnergyStore(100F, 1F);
        }

        public IShield CreateShield(IShip ship)
        {
            return new Shield(ship, 100F, 0.1F);
        }

        public IHull CreateHull(IGameObject ship)
        {
            return new Hull(ship, 100F);
        }

        public IThrusterArray CreateThrusterArray(IShip ship)
        {
            return new ThrusterArray(ship);
        }
    }
}
