namespace DEMW.SpaceWar2.Core.Utils.XnaWrappers
{
    public interface IContentManager
    {
        T Load<T>(string assetName);
    }
}