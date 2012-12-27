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

        public Arrow CreateAccelerationArrow(Vector2 position, Vector2 direction, float radius)
        {
            return new Arrow(_graphics, position, direction, Color.LimeGreen, radius);
        }

        public Arrow CreateVelocityArrow(Vector2 position, Vector2 direction, float radius)
        {
            return new Arrow(_graphics, position, direction, Color.Linen, radius);
        }

        public Arrow CreateRotationArrow(Vector2 position, Vector2 direction, float radius)
        {
            return new Arrow(_graphics, position, direction, Color.Red, radius);
        }

        public Arrow CreateForceArrow(Vector2 position, Vector2 direction, float radius)
        {
            return new Arrow(_graphics, position, direction, Color.Yellow, radius);
        }
    }
}
