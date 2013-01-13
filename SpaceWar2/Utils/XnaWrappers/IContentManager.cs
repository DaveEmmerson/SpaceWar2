using System;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public interface IContentManager : IDisposable
    {
        T Load<T>(string assetName);
    }
}