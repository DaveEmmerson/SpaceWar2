﻿using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    public class KeyboardHandler : IKeyboardHandler
    {
        private readonly IKeyboard _keyboard;
        private KeyboardState _oldKeyboardState;
        private KeyboardState _keyboardState;

        public KeyboardHandler(IKeyboard keyboard)
        {
            _keyboard = keyboard;
            _keyboardState = _keyboard.GetState();
            _oldKeyboardState = _keyboardState;
        }

        public void UpdateKeyboardState() 
        {
            _oldKeyboardState = _keyboardState;
            _keyboardState = _keyboard.GetState();
        }

        public bool IsPressed(Keys key)
        {
            return _keyboardState.IsKeyDown(key);
        }

        public bool IsNewlyPressed(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && !_oldKeyboardState.IsKeyDown(key);
        }
    }
}
