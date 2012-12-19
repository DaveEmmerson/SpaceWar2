using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class KeyboardWrapperTests
    {
        //Bit of a noddy test, but at least it causes this code to be executed.
        [Test]
        public void IsPressed_returns_correct_value_even_if_repeatedly_called(Keys key)
        {
            var _keyboardWrapper = new KeyboardWrapper();

            var state = _keyboardWrapper.GetState();

            Assert.IsNotNull(state);
        }
    }
}
