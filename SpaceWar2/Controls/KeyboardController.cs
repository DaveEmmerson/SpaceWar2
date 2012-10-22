using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    class KeyboardController : IShipController
    {
        private readonly IDictionary<Keys, ShipAction> _mappings;
        private readonly KeyboardHandler _keyboardHandler;

        public KeyboardController(KeyboardHandler keyboardHandler)
        {
            _keyboardHandler = keyboardHandler;
            _mappings = new Dictionary<Keys, ShipAction>();
        }

        public void SetMapping(Keys key, ShipAction shipAction)
        {
            var existingMappings = _mappings.Where(x=> x.Value == shipAction);
            foreach (var mapping in existingMappings)
            {
                _mappings.Remove(mapping.Key);
            }

            _mappings[key] = shipAction;
        }

        public ShipAction GetAction()
        {
            return _mappings
                .Where(mapping => _keyboardHandler.IsPressed(mapping.Key))
                .Aggregate(ShipAction.None, (current, mapping) => current | mapping.Value);
        }
    }
}
