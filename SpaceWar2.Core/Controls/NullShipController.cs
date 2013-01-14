namespace DEMW.SpaceWar2.Core.Controls
{
    internal class NullShipController : IShipController
    {
        public ShipActions Actions
        {
            get { return ShipActions.None; }
        }
    }
}
