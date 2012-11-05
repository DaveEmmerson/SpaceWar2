using NUnit.Framework;
using DEMW.SpaceWar2;
using System;

namespace DEMW.SpaceWar2Tests
{
    [TestFixture]
    class GameObjectFactoryTests
    {
        private GameObjectFactory _gameObjectFactory;

        [Test]
        public void Constructor_throws_null_reference_exception_if_any_argument_is_null()
        {
            Assert.Throws<NullReferenceException>(()=> _gameObjectFactory = new GameObjectFactory(null, null, null, null, null));
        }
    }
}
