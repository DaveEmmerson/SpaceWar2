using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public interface IGraphicsFactory
    {
        Arrow CreateArrow(Vector2 position, Vector2 direction, Color color, float radius);
    }
}