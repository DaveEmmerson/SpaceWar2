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
