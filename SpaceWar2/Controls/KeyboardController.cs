using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Core.Controls;
using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    internal class KeyboardController : IShipController
    {
        private readonly IDictionary<Keys, ShipActions> _mappings;
        private readonly IKeyboardHandler _keyboardHandler;

        internal KeyboardController(IKeyboardHandler keyboardHandler)
        {
            _keyboardHandler = keyboardHandler;
            _mappings = new Dictionary<Keys, ShipActions>();
        }

        internal void SetMapping(Keys key, ShipActions shipActions)
        {
            var existingMappings = _mappings.Where(x=> x.Value == shipActions).ToList();
            foreach (var mapping in existingMappings)
            {
                _mappings.Remove(mapping.Key);
            }

            _mappings[key] = shipActions;
        }

        public ShipActions Actions
        {
            get
            {
                return _mappings
                    .Where(mapping => _keyboardHandler.IsPressed(mapping.Key))
                    .Aggregate(ShipActions.None, (current, mapping) => current | mapping.Value);
            }
        }
    }
}
