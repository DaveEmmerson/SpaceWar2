using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    class GraphicsFactory : IGraphicsFactory
    {
        private readonly IGraphicsDeviceManager _graphics;

        public GraphicsFactory(IGraphicsDeviceManager graphics)
        {
            _graphics = graphics as GraphicsDeviceManager;
        }

        public Arrow CreateArrow(Vector2 position, Vector2 direction, Color color, float radius)
        {
            return new Arrow(_graphics, position, direction, color, radius);
        }
    }
}
