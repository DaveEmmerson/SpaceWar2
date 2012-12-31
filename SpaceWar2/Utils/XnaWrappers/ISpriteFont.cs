using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public interface ISpriteFont
    {
        SpriteFont SpriteFont { get; }
        int LineSpacing { get; }
    }
}