using FiascoRL.Display.UI.Controls.Coordinates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class TitleBarControl : Control
    {
        /// <summary>
        /// Creates a new title bar control with the specified text.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public TitleBarControl(string text)
            : base()
        {
            this.Text = text;
            this._textDimensions = UI.UIGraphic.FiascoFont.MeasureString(Text);
            this.WBitmap = new Rectangle(634, 118, 24, 24);
            this.CenterBitmap = new Rectangle(659, 118, 23, 24);
            this.EBitmap = new Rectangle(683, 118, 25, 24);
        }

        /// <summary>
        /// Text displayed on this titlebar.
        /// </summary>
        public string Text { get; set; }

        public override Control Parent
        {
            get
            {
                return _parent;
            }
            set 
            {
                // Check if coords has been initialized.
                if (this.Coords.Width == default(float) && this.Coords.Height == default(float))
                {
                    // Have to get actual value of parent's coords.
                    this.Coords = value.Coords;
                    Rectangle rect = GetActualCoords();

                    this.Coords = new RelativeRectangle(
                        new RelativeVector(0.125f, 0.0f),
                        new RelativeVector(0.0f, -10.0f),
                        0.75f,
                        CenterBitmap.Height / rect.Height);
                }

                _parent = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Nothing to update.
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Rectangle rect = GetActualCoords();

            // Draw title text.
            float x = rect.X + rect.Width / 2 - _textDimensions.X / 2;
            float y = rect.Y + 9;
            UIGraphic.DrawBorderText(spriteBatch, UIGraphic.FiascoFont, Text, x, y, 1);
        }

        private Control _parent;
        private Vector2 _textDimensions;
        
    }
}
