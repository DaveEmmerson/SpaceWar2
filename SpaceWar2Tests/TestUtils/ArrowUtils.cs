using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Utils;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    internal static class ArrowUtils
    {
        public static float GetLength(IArrow arrow)
        {
            var arrowObject = (Arrow) arrow;
            var x0 = arrowObject.Verticies[0].Position.X;
            var y0 = arrowObject.Verticies[0].Position.Y;
            var x3 = arrowObject.Verticies[3].Position.X;
            var y3 = arrowObject.Verticies[3].Position.Y;
            var actualLength = new Vector2(x3 - x0, y3 - y0).Length();
            return actualLength;
        }

        public static void CheckColour(IArrow arrow, Color color)
        {
            var arrowObject = (Arrow) arrow;

            Assert.AreEqual(Color.Transparent, arrowObject.Verticies[0].Color);
            Assert.AreEqual(color, arrowObject.Verticies[1].Color);
            Assert.AreEqual(color, arrowObject.Verticies[2].Color);
            Assert.AreEqual(color, arrowObject.Verticies[3].Color);
            Assert.AreEqual(color, arrowObject.Verticies[4].Color);
            Assert.AreEqual(color, arrowObject.Verticies[5].Color);
        }

        public static Vector2 GetDirection(IArrow arrow)
        {
            var arrowObject = (Arrow) arrow;

            var actualDirection = arrowObject.Verticies[1].Position - arrowObject.Verticies[0].Position;
            actualDirection.Normalize();
            
            return actualDirection.DiscardZComponent();
        }
    }
}