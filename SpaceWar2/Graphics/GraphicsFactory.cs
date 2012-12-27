using System;
using DEMW.SpaceWar2.Physics;
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

        public Arrow CreateAccelerationArrow(Vector2 acceleration, float radius)
        {
            return new Arrow(_graphics, Vector2.Zero, acceleration, Color.LimeGreen, radius);
        }

        public Arrow CreateVelocityArrow(Vector2 direction, float radius)
        {
            return new Arrow(_graphics, Vector2.Zero, direction, Color.Linen, radius);
        }

        public Arrow CreateRotationArrow(float rotation, float radius)
        {
            var x = (float)Math.Sin(rotation) * radius * 2;
            var y = -(float)Math.Cos(rotation) * radius * 2;

            var rotationAngle = new Vector2(x, y);
            return new Arrow(_graphics, Vector2.Zero, rotationAngle, Color.Linen, radius);
        }

        public Arrow CreateForceArrow(Force force, float radius)
        {
            return new Arrow(_graphics, force.Displacement, force.Vector, Color.Yellow, radius);
        }
    }
}
