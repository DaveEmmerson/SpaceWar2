using System.Globalization;
using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DEMW.SpaceWar2.Graphics
{
    class InfoBar
    {
        private readonly SpriteBatch _spriteBatch;

        private readonly SpriteFont _font;

        private Vector2 _currentPosition;

        public Vector2 Position { get; set; }

        public Color FontColor { get; set; }

        public InfoBar(SpriteBatch spriteBatch, ContentManager contentManager) 
        {
            _spriteBatch = spriteBatch;
            _font = contentManager.Load<SpriteFont>("Fonts/Segoe UI Mono");

            Position = new Vector2(10, 10);

            Reset();

            FontColor = Color.LightBlue;
        }

        public void Reset()
        {
            _currentPosition = Position;
        }
        
        public void DrawShipInfo(Ship ship)
        {
            DrawString(ship.Name + ".Position.X", ship.Position.X.ToString(CultureInfo.InvariantCulture));
            DrawString(ship.Name + ".Position.Y", ship.Position.Y.ToString(CultureInfo.InvariantCulture));
            DrawString(ship.Name + ".Velocity.X", ship.Velocity.X.ToString(CultureInfo.InvariantCulture));
            DrawString(ship.Name + ".Velocity.Y", ship.Velocity.Y.ToString(CultureInfo.InvariantCulture));
            LineBreak();
            DrawString(ship.Name + ".Rotation", ship.Rotation.ToString(CultureInfo.InvariantCulture));
            DrawString(ship.Name + ".AngularVelocity", ship.AngularVelocity.ToString(CultureInfo.InvariantCulture));
            LineBreak();
            DrawString(ship.Name + ".Shields", ship.Shields.ToString(CultureInfo.InvariantCulture));
            DrawString(ship.Name + ".Armour", ship.Armour.ToString(CultureInfo.InvariantCulture));
            DrawString(ship.Name + ".Energy", ship.Energy.ToString(CultureInfo.InvariantCulture));

            LineBreak();
            LineBreak();
            LineBreak();
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
