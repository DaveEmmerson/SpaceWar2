using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Utils.XnaWrappers
{
    public class SpriteFontWrapper : ISpriteFont
    {
        public SpriteFontWrapper(SpriteFont spriteFont)
        {
            SpriteFont = spriteFont;
        }

        public SpriteFont SpriteFont { get; private set; }

        public int LineSpacing
        {
            get { return SpriteFont.LineSpacing; }
        }
    }
}