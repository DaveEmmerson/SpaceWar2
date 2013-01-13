using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    internal class InfoBar
    {
        private readonly ISpriteBatch _spriteBatch;
        private readonly ISpriteFont _spriteFont;

        private Color FontColor { get; set; }

        internal InfoBar(ISpriteBatch spriteBatch, ISpriteFont spriteFont)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            
            FontColor = Color.LightBlue;
        }

        internal Vector2 CursorPosition { get; set; }

        internal void DrawString(string output)
        {
            _spriteBatch.BeginBatch();
            _spriteBatch.DrawString(_spriteFont, output, CursorPosition, FontColor);
            _spriteBatch.EndBatch();

            var noOfLines = NoOfLines(output);
            CursorPosition += new Vector2(0, _spriteFont.LineSpacing * noOfLines);
        }

        /// <summary>
        /// Nabbed from an article on Codeproject:
        /// http://www.codeproject.com/Tips/312312/Counting-lines-in-a-string
        /// </summary>
        private static long NoOfLines(string message)
        {
            var count = 1;
            var position = -1;
            while ((position = message.IndexOf('\n', position + 1)) != -1) { count++; }
            return count;
        }
    }
}
