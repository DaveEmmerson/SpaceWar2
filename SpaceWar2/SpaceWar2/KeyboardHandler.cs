using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceWar2
{

    class KeyboardHandler
    {
        private KeyboardState oldKeyboardState;
        private KeyboardState keyboardState;

        public KeyboardHandler()
        {

            keyboardState = Keyboard.GetState();
            oldKeyboardState = keyboardState;

        }

        public void UpdateKeyboardState() {

            oldKeyboardState = keyboardState;

            keyboardState = Keyboard.GetState();

        }

        public bool IsPressed(Keys key)
        {

            return keyboardState.IsKeyDown(key);

        }

        public bool IsNewlyPressed(Keys key)
        {

            return keyboardState.IsKeyDown(key) && !oldKeyboardState.IsKeyDown(key);

        }
    }
}
