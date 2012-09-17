using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceWar2
{
    interface IMassive
    {

        float Mass { get; set; }

        Vector2 Position { get; set; }

        float Radius { get; set; }

    }
}
