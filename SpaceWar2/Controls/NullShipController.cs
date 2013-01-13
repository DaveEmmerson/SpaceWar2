namespace DEMW.SpaceWar2.Controls
{
    public class NullShipController : IShipController
    {
        public ShipActions Actions
        {
            get { return ShipActions.None; }
        }
    }
}
