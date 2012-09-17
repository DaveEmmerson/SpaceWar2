using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceWar2
{ 
    class Sun : Circle, IMassive
    {

        public float Mass { get; set; }

        public Sun(GraphicsDeviceManager graphics, Vector2 position, float radius, Color lineColor, uint lineCount, float mass)
            : base(graphics, position, radius, lineColor, lineCount)
        {

            Mass = mass;

        }
    }
}
