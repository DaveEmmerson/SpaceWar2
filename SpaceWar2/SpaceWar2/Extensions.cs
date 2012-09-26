using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWar2
{
    public static class MyExtensions
    {
        public static void ForEach<TBase, TSubClass>(this List<TBase> items, Action<TSubClass> action)
            where TBase : class
            where TSubClass : class,TBase
        {
            foreach (var item in items.OfType<TSubClass>())
            {
                action(item);
            }
        }
    }
}
