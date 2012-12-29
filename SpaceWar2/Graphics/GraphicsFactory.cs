﻿using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2.Utils;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public class GraphicsFactory : IGraphicsFactory
    {
        public IArrow CreateAccelerationArrow(Vector2 acceleration, float radius)
        {
            return Arrow.CreateArrow(Vector2.Zero, acceleration, Color.LimeGreen, radius);
        }

        public IArrow CreateVelocityArrow(Vector2 direction, float radius)
        {
            return Arrow.CreateArrow(Vector2.Zero, direction, Color.Linen, radius);
        }

        public IArrow CreateRotationArrow(float rotation, float radius)
        {
            var vector = -Vector2.UnitY * radius * 2f;
            var rotationAngle = vector.Rotate(rotation);

            return Arrow.CreateArrow(Vector2.Zero, rotationAngle, Color.Red, radius);
        }

        public IArrow CreateForceArrow(Force force, float radius)
        {
            return Arrow.CreateArrow(force.Displacement, force.Vector, Color.Yellow, radius);
        }
    }
}
