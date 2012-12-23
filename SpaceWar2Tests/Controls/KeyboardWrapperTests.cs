using DEMW.SpaceWar2.Controls;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Controls
{
    [TestFixture]
    class KeyboardWrapperTests
    {
        private KeyboardState x;
        //Bit of a noddy test, but at least it causes this code to be executed.
        [Test]
        public void GetState_returns_does_not_throw_exception()
        {
            var _keyboardWrapper = new KeyboardWrapper();

            Assert.DoesNotThrow(() => x = _keyboardWrapper.GetState());
        }

    }
}
