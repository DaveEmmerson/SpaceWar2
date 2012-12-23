using System;
using Microsoft.Xna.Framework.Content;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public class ContentManagerWrapper : IContentManager, IDisposable
    {
        private readonly ContentManager _contentManager;

        public ContentManagerWrapper(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public T Load<T>(string assetName)
        {
            return _contentManager.Load<T>(assetName);
        }

        public string RootDirectory
        {
            get { return _contentManager.RootDirectory; }
            set { _contentManager.RootDirectory = value; }
        }

        public void Dispose()
        {
            _contentManager.Dispose();
        }
    }
}
