using DEMW.SpaceWar2.Core.Graphics;
using DEMW.SpaceWar2.Core.Utils;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    internal static class ArrowUtils
    {
        public static float GetLength(IArrow arrow)
        {
            var arrowObject = (Arrow) arrow;

            var x0 = arrowObject.Vertices()[0].Position.X;
            var y0 = arrowObject.Vertices()[0].Position.Y;
            var x3 = arrowObject.Vertices()[3].Position.X;
            var y3 = arrowObject.Vertices()[3].Position.Y;

            return new Vector2(x3 - x0, y3 - y0).Length();
        }

        public static void CheckColour(IArrow arrow, Color color)
        {
            var arrowObject = (Arrow) arrow;

            Assert.AreEqual(Color.Transparent, arrowObject.Vertices()[0].Color);
            Assert.AreEqual(color, arrowObject.Vertices()[1].Color);
            Assert.AreEqual(color, arrowObject.Vertices()[2].Color);
            Assert.AreEqual(color, arrowObject.Vertices()[3].Color);
            Assert.AreEqual(color, arrowObject.Vertices()[4].Color);
            Assert.AreEqual(color, arrowObject.Vertices()[5].Color);
        }

        public static Vector2 GetPosition(IArrow arrow)
        {
            var arrowObject = (Arrow)arrow;

            return new Vector2(arrowObject.Vertices()[0].Position.X, arrowObject.Vertices()[0].Position.Y);
        }

        public static Vector2 GetDirection(IArrow arrow)
        {
            var arrowObject = (Arrow) arrow;

            var actualDirection = arrowObject.Vertices()[1].Position - arrowObject.Vertices()[0].Position;
            actualDirection.Normalize();
            
            return actualDirection.DiscardZComponent();
        }
    }
}