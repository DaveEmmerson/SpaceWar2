using Microsoft.Xna.Framework.Input;
using System;

namespace DEMW.SpaceWar2.Core.Controls
{
    public interface IActionHandler
    {
        void ProcessActions();

        /// <summary>
        /// Register an action that will occur once when a key is press
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        void RegisterTriggerAction(Keys key, Action action);

        /// <summary>
        /// Register an action that will occur while a key is pressed
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        void RegisterContinuousAction(Keys key, Action action);
    }
}