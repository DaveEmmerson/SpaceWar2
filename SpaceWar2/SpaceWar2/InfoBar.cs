using System.Globalization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpaceWar2
{
    class InfoBar
    {
        private readonly SpriteBatch _spriteBatch;

        private SpriteFont _font;

        private Vector2 _currentPosition;

        public Vector2 Position { get; set; }

        public Color FontColor { get; set; }
        
        public InfoBar(SpriteBatch spriteBatch) 
        {
            _spriteBatch = spriteBatch;

            Position = new Vector2(10, 10);

            Reset();

            FontColor = Color.LightBlue;
        }

        public void Reset()
        {
            _currentPosition = Position;
        }

        public void LoadContent(ContentManager contentManager) 
        {
            _font = contentManager.Load<SpriteFont>("Fonts/Segoe UI Mono");
        }
        
        public void DrawShipInfo(Ship circle)
        {
            DrawString(circle.Name + ".Position.X", circle.Position.X.ToString(CultureInfo.InvariantCulture));
            DrawString(circle.Name + ".Position.Y", circle.Position.Y.ToString(CultureInfo.InvariantCulture));
            DrawString(circle.Name + ".Shields", circle.Shields.ToString(CultureInfo.InvariantCulture));
            DrawString(circle.Name + ".Armour", circle.Armour.ToString(CultureInfo.InvariantCulture));
            DrawString(circle.Name + ".Energy", circle.Energy.ToString(CultureInfo.InvariantCulture));

            LineBreak();
        }

        public void DrawString(string heading, string value)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, heading + ": " + value, _currentPosition, FontColor);
            _spriteBatch.End();

            LineBreak();
        }

        public void LineBreak()
        {
            _currentPosition += new Vector2(0, _font.LineSpacing);
        }
    }
}
