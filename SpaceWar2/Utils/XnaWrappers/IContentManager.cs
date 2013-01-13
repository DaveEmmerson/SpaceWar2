namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public interface IContentManager
    {
        T Load<T>(string assetName);
    }
}