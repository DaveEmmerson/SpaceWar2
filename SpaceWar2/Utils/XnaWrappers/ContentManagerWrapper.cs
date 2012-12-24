using System;
using Microsoft.Xna.Framework.Content;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public class ContentManagerWrapper : ContentManager, IContentManager
    {
        public ContentManagerWrapper(IServiceProvider serviceProvider, string rootDirectory)
            : base(serviceProvider, rootDirectory)
        {
        }
    }
}
