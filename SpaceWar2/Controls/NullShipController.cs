namespace DEMW.SpaceWar2.Controls
{
    class NullShipController : IShipController
    {
        public ShipAction GetAction()
        {
            return ShipAction.None;
        }
    }
}
