using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpaceWar2
{
    class InfoBar
    {

        private SpriteBatch spriteBatch;

        private SpriteFont font;

        private Vector2 currentPosition;

        public Vector2 Position { get; set; }

        public Color FontColor { get; set; }
        

        public InfoBar(SpriteBatch spriteBatch) {

            this.spriteBatch = spriteBatch;

            Position = new Vector2(10, 10);

            Reset();

            FontColor = Color.LightBlue;

        }

        public void Reset()
        {

            currentPosition = Position;

        }

        public void LoadContent(ContentManager contentManager) {

            font = contentManager.Load<SpriteFont>("Segoe UI Mono");

        }
        
        public void DrawString(string heading, string value) {

            spriteBatch.Begin();

            spriteBatch.DrawString(font, heading + ": " + value , currentPosition, FontColor);

            spriteBatch.End();

            LineBreak();
        }

        public void LineBreak()
        {
            currentPosition += new Vector2(0, font.LineSpacing);
        }


        public void DrawShipInfo(Ship circle)
        {

            DrawString(circle.Name + ".Position.X", circle.Position.X.ToString());
            DrawString(circle.Name + ".Position.Y", circle.Position.Y.ToString());
            DrawString(circle.Name + ".Shields", circle.Shields.ToString());
            DrawString(circle.Name + ".Armour", circle.Armour.ToString());
            DrawString(circle.Name + ".Energy", circle.Energy.ToString());


            LineBreak();

        }
    }
}
