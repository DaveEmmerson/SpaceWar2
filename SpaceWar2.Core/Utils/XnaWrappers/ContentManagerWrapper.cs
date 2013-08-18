using Microsoft.Xna.Framework.Content;
using System;

namespace DEMW.SpaceWar2.Core.Utils.XnaWrappers
{
    internal class ContentManagerWrapper : ContentManager, IContentManager
    {
        internal ContentManagerWrapper(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }
    }
}
