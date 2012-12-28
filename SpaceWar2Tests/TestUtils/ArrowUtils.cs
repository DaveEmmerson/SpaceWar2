using DEMW.SpaceWar2.Graphics;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2Tests.TestUtils
{
    static internal class ArrowUtils
    {
        public static float GetArrowLength(IArrow arrow)
        {
            var arrowObject = (Arrow) arrow;
            var x0 = arrowObject.Verticies[0].Position.X;
            var y0 = arrowObject.Verticies[0].Position.Y;
            var x3 = arrowObject.Verticies[3].Position.X;
            var y3 = arrowObject.Verticies[3].Position.Y;
            var actualLength = new Vector2(x3 - x0, y3 - y0).Length();
            return actualLength;
        }
    }
}