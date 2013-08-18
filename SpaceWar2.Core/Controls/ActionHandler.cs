using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DEMW.SpaceWar2.Core.Controls
{
    internal class ActionHandler : IActionHandler
    {
        private readonly IKeyboardHandler _keyboardHandler;
        
        private readonly IDictionary<Keys, Action> _triggerActions;
        private readonly IDictionary<Keys, Action> _continuousActions;

        internal ActionHandler(IKeyboardHandler keyboardHandler)
        {
            _keyboardHandler = keyboardHandler;
            _triggerActions = new Dictionary<Keys, Action>();
            _continuousActions = new Dictionary<Keys, Action>();
        }

        public void RegisterTriggerAction(Keys key, Action action)
        {
            _triggerActions[key] = action;
        }

        public void RegisterContinuousAction(Keys key, Action action)
        {
            _continuousActions[key] = action;
        }

        public void ProcessActions()
        {
            foreach (var action in _triggerActions.Where(x => _keyboardHandler.IsNewlyPressed(x.Key)))
            {
                action.Value();
            }

            foreach (var action in _continuousActions.Where(x => _keyboardHandler.IsPressed(x.Key)))
            {
                action.Value();
            }
        }
    }
}
