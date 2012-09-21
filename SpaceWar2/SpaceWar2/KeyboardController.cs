using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace SpaceWar2
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
            var action = ShipAction.None;

            foreach (var mapping in _mappings)
            {
                if (_keyboardHandler.IsPressed(mapping.Key))
                {
                    action |= mapping.Value;
                }
            }

            return action;
        }
    }
}
