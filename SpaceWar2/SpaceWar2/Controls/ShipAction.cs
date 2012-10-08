using System;

namespace DEMW.SpaceWar2.Controls
{
    [Flags]
    public enum ShipAction
    {
        None = 0,
        Thrust = 1,
        TurnLeft = 2,
        TurnRight = 4,
        ReverseThrust = 8,
        FireProjectile = 16
    }
}
