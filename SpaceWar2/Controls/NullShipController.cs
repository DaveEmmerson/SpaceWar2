using DEMW.SpaceWar2.Core.Controls;

namespace DEMW.SpaceWar2.Controls
{
    internal class NullShipController : IShipController
    {
        public ShipActions Actions
        {
            get { return ShipActions.None; }
        }
    }
}
