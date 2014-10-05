using FiascoRL.Display.UI.Controls.Coordinates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class RaisedButtonControl : Control
    {
        public RaisedButtonControl(string text)
        {
            this.Text = text;
            this.WBitmap = new Rectangle(634, 118, 24, 24);
            this.CenterBitmap = new Rectangle(659, 118, 23, 24);
            this.EBitmap = new Rectangle(683, 118, 25, 24);
        }

        /// <summary>
        /// Text displayed on this button.
        /// </summary>
        public string Text { get; set; }

        public Action OnClick { get; set; }

        public void SetPositionAndAutoSize(Point position)
        {
            Vector2 textDimensions = UIGraphic.FiascoFont.MeasureString(Text);
            this.Coords = new RelativeRectangle(
                new RelativeVector(0.0f, (float)position.X),
                new RelativeVector(0.0f, (float)position.Y),
                (textDimensions.X + 12) / Parent.GetActualCoords().Width,
                24f / Parent.GetActualCoords().Height);
        }

        public void SetPositionAndAutoSize(RelativeVector x, RelativeVector y)
        {
            Vector2 textDimensions = UIGraphic.FiascoFont.MeasureString(Text);
            this.Coords = new RelativeRectangle(
                x,
                y,
                (textDimensions.X + 30) / Parent.GetActualCoords().Width,
                24f / Parent.GetActualCoords().Height);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Parent == null)
                return;

            MouseState ms = Mouse.GetState();
            Rectangle actualCoords = GetActualCoords();
            Point currentMousePos = new Point(ms.X, ms.Y);

            if (actualCoords.Contains(currentMousePos) && ms.LeftButton == ButtonState.Pressed && !_clicked)
            {
                _clicked = true;
                OnClick();
            }
            else if (actualCoords.Contains(currentMousePos))
            {
                //Hovering = true;
                //Selected = false;
            }
            else
            {
                //Hovering = false;
                //Selected = false;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 textDimensions = UIGraphic.FiascoFont.MeasureString(Text);
            Rectangle rect = GetActualCoords();

            // Draw title text.
            float x = rect.X + rect.Width / 2 - textDimensions.X / 2;
            float y = rect.Y + 9;
            UIGraphic.DrawBorderText(spriteBatch, UIGraphic.FiascoFont, Text, x, y, 1);
        }
        private bool _clicked;
    }
}
