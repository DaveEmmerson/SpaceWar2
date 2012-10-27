namespace DEMW.SpaceWar2.Controls
{
    public class NullShipController : IShipController
    {
        public ShipAction GetAction()
        {
            return ShipAction.None;
        }
    }
}
