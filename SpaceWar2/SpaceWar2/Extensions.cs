using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceWar2
{
    public static class MyExtensions
    {
        public static void ForEach<B, S>(this List<B> items, Action<S> action)
            where B : class
            where S : class,B
        {

            foreach (var item in items.OfType<S>())
            {
                action(item);
            }
        }
    }
}
