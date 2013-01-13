using System;
using Microsoft.Xna.Framework.Content;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    internal class ContentManagerWrapper : ContentManager, IContentManager
    {
        internal ContentManagerWrapper(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }
    }
}
