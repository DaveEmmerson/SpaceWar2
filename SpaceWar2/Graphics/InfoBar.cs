using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DEMW.SpaceWar2.Graphics
{
    class InfoBar
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _font;
        private Color FontColor { get; set; }
        private Vector2 _currentPosition;
        
        public InfoBar(SpriteBatch spriteBatch, IContentManager contentManager) 
        {
            _spriteBatch = spriteBatch;
            _font = contentManager.Load<SpriteFont>("Fonts/Segoe UI Mono");
            FontColor = Color.LightBlue;

            Reset();
        }

        public void Reset()
        {
            _currentPosition = new Vector2(10,10);
        }
        
        public void DrawString(string message)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, message, _currentPosition, FontColor);
            _spriteBatch.End();

            var noOfLines = NoOfLines(message);
            _currentPosition += new Vector2(0, _font.LineSpacing * noOfLines);
        }

        /// <summary>
        /// Nabbed from an article on Codeproject:
        /// http://www.codeproject.com/Tips/312312/Counting-lines-in-a-string
        /// </summary>
        static long NoOfLines(string message)
        {
            var count = 0;
            var position = -1;
            while ((position = message.IndexOf('\n', position + 1)) != -1) { count++; }
            return count;
        }
    }
}
