using DEMW.SpaceWar2.Controls;

namespace DEMW.SpaceWar2.GameObjects.ShipComponents
{
    public interface IThrusterArray
    {
        void CalculateThrustPattern(ShipAction action);
        void EngageThrusters();
    }
}