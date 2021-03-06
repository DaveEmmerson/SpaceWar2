﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DEMW.SpaceWar2.Core.Utils
{
    internal static class Extensions
    {
        internal static void ForEach<TBase, TSubclass>(this IEnumerable<TBase> items, Action<TSubclass> action)
            where TBase : class
            where TSubclass : class,TBase
        {
            if (action == null) return;

            foreach (var item in items.OfType<TSubclass>())
            {
                action(item);
            }
        }

        internal static Vector2 Rotate(this Vector2 vector, float rotation)
        {
            var rotationMatrix = Matrix.CreateRotationZ(rotation);
            return Vector2.Transform(vector, rotationMatrix);
        }

        internal static Vector2 DiscardZComponent(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }
    }
}
