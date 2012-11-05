using Microsoft.Xna.Framework.Input;

namespace DEMW.SpaceWar2.Controls
{
    public class KeyboardHandler : IKeyboardHandler
    {
        public IKeyboard Keyboard { get; set; }
        
        private KeyboardState _oldKeyboardState;
        private KeyboardState _keyboardState;

        public KeyboardHandler()
        {
            Keyboard = new KeyboardWrapper();
            _keyboardState = Keyboard.GetState();
            _oldKeyboardState = _keyboardState;
        }

        public void UpdateKeyboardState() 
        {
            _oldKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();
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
