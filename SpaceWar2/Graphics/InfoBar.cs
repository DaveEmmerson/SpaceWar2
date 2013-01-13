using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Graphics
{
    public class InfoBar
    {
        private readonly ISpriteBatch _spriteBatch;
        private readonly ISpriteFont _spriteFont;

        private Color FontColor { get; set; }

        public InfoBar(ISpriteBatch spriteBatch, ISpriteFont spriteFont)
        {
            _spriteBatch = spriteBatch;
            _spriteFont = spriteFont;
            
            FontColor = Color.LightBlue;
        }

        public Vector2 CursorPosition { get; set; }
        
        public void DrawString(string message)
        {
            _spriteBatch.BeginBatch();
            _spriteBatch.DrawString(_spriteFont, message, CursorPosition, FontColor);
            _spriteBatch.EndBatch();

            var noOfLines = NoOfLines(message);
            CursorPosition += new Vector2(0, _spriteFont.LineSpacing * noOfLines);
        }

        /// <summary>
        /// Nabbed from an article on Codeproject:
        /// http://www.codeproject.com/Tips/312312/Counting-lines-in-a-string
        /// </summary>
        static long NoOfLines(string message)
        {
            var count = 1;
            var position = -1;
            while ((position = message.IndexOf('\n', position + 1)) != -1) { count++; }
            return count;
        }
    }
}
