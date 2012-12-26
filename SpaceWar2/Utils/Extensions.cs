using System;
using System.Collections.Generic;
using System.Linq;

namespace DEMW.SpaceWar2.Utils
{
    public static class Extensions
    {
        public static void ForEach<TBase, TSubClass>(this IEnumerable<TBase> items, Action<TSubClass> action)
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
