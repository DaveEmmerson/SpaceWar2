namespace DEMW.SpaceWar2.Controls
{
    public class NullShipController : IShipController
    {
        public ShipAction Action
        {
            get { return ShipAction.None; }
        }
    }
}
