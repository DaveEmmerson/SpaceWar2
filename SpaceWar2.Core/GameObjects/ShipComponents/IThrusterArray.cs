using DEMW.SpaceWar2.Core.Controls;

namespace DEMW.SpaceWar2.Core.GameObjects.ShipComponents
{
    public interface IThrusterArray
    {
        void CalculateThrustPattern(ShipActions actions);
        void EngageThrusters();
    }
}