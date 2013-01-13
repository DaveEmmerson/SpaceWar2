using DEMW.SpaceWar2.Graphics;
using DEMW.SpaceWar2.Utils.XnaWrappers;
using Microsoft.Xna.Framework;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Graphics
{
    [TestFixture]
    internal class InfoBarTests
    {
        private ISpriteBatch _spriteBatch;
        private ISpriteFont _spriteFont;

        private readonly Color _expectedColor = Color.LightBlue;

        [SetUp]
        public void SetUp()
        {
            _spriteBatch = Substitute.For<ISpriteBatch>();
            _spriteFont = Substitute.For<ISpriteFont>();
        }

        [Test]
        public void InfoBar_creates_new_infobar_with_default_properties()
        {
            var infoBar = new InfoBar(_spriteBatch, _spriteFont);
            
            Assert.AreEqual(infoBar.CursorPosition, Vector2.Zero);
        }

        [Test]
        public void DrawString_calls_DrawString_on_SpriteBatch_and_moves_down_one_line_when_called_with_one_line()
        {
            _spriteFont.LineSpacing.Returns(15);
            var infoBar = new InfoBar(_spriteBatch, _spriteFont);

            const string text = "Test Message";
            infoBar.DrawString(text);

            var expectedStartPosition = Vector2.Zero;

            _spriteBatch.Received(1).BeginBatch();
            _spriteBatch.Received(1).DrawString(_spriteFont, text, expectedStartPosition, _expectedColor);
            _spriteBatch.Received(1).EndBatch();

            var expectedEndPosition = expectedStartPosition + new Vector2(0, 15);
            Assert.AreEqual(expectedEndPosition, infoBar.CursorPosition);
        }

        [Test]
        public void DrawString_calls_DrawString_on_SpriteBatch_and_moves_cursor_down_multiple_lines_when_called_with_multiline_text()
        {
            _spriteFont.LineSpacing.Returns(15);
            var infoBar = new InfoBar(_spriteBatch, _spriteFont);

            const string text = 
@"Test Message

On multiple Lines";
            infoBar.DrawString(text);

            var expectedStartPosition = Vector2.Zero;

            _spriteBatch.Received(1).BeginBatch();
            _spriteBatch.Received(1).DrawString(_spriteFont, text, expectedStartPosition, _expectedColor);
            _spriteBatch.Received(1).EndBatch();

            const int noOfLines = 3;
            var expectedEndPosition = expectedStartPosition + new Vector2(0, noOfLines * 15);
            Assert.AreEqual(expectedEndPosition, infoBar.CursorPosition);
        }

        [Test]
        public void CursorPosition_sets_the_cursor_position()
        {
            var infoBar = new InfoBar(_spriteBatch, _spriteFont);

            var expectedPosition = new Vector2(59, 100);
            infoBar.CursorPosition = expectedPosition;

            Assert.AreEqual(expectedPosition, infoBar.CursorPosition);
        }
    }
}
