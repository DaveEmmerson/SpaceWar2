using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects
{
    [TestFixture]
    internal class SunTests
    {
        [Test]
        public void Sun_can_be_created_and_its_properties_are_set_in_the_constructor()
        {
            var position = new Vector2(12f, 5.5f);
            var sun = new Sun(position, 30f, Color.Goldenrod, 200f);

            Assert.AreEqual(position, sun.Position);
            Assert.AreEqual(30f, sun.Radius);
            Assert.AreEqual(Color.Goldenrod, sun.Color);
            Assert.AreEqual(200f, sun.Mass);
        }

        [Test]
        public void UpdateInternal_does_nothing() //for now
        {
            var position = new Vector2(12f, 5.5f);
            var sun = new Sun(position, 30f, Color.Goldenrod, 200f);

            sun.Update(new GameTime());
        }

        [Test]
        public void Draw_does_nothing() //for now
        {
            var position = new Vector2(12f, 5.5f);
            var sun = new Sun(position, 30f, Color.Goldenrod, 200f);

            sun.Draw();
        }
    }
}
