using FiascoRL.Display.UI.Controls.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class HoverableIconControl : Control
    {
        public HoverableIconControl(Texture2D texture, Rectangle sprite, string text)
        {
            this._texture = texture;
            this._text = text;
            this._sprite = sprite;
            this.CenterBitmap = new Rectangle();
            this._transformation = Matrix.CreateScale(2.0f);
            this._textOffset = UIGraphic.FiascoFontSmall.MeasureString(text);
        }

        /// <summary>
        /// Group this icon is associated with.
        /// </summary>
        public string IconGroup { get; set; }

        /// <summary>
        /// Whether or not the mouse is currently over this icon.
        /// </summary>
        public bool Hovering { get; private set; }

        /// <summary>
        /// Whether or not this icon is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Index used for drawing this icon.
        /// </summary>
        public int IconIndex { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Parent == null)
                return;

            MouseState ms = Mouse.GetState();
            Rectangle rect = GetActualCoords();
            Rectangle actualCoords = new Rectangle(rect.X * 2, rect.Y * 2, rect.Width * 2, rect.Height * 2);
            Point currentMousePos = new Point(ms.X, ms.Y);

            if ((actualCoords.Contains(currentMousePos) && ms.LeftButton == ButtonState.Pressed) || Selected)
            {
                ((IHoverableIconHandler)Parent).DeselectAll();
                Selected = true;
            }
            else if (actualCoords.Contains(currentMousePos))
            {
                Hovering = true;
                Selected = false;
            }
            else
            {
                Hovering = false;
                Selected = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle actualCoords = GetActualCoords();
            spriteBatch.End();
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                SamplerState.PointClamp,
                null,
                null,
                null,
                _transformation);

            spriteBatch.Draw(_texture, actualCoords, _sprite, Color.White);

            Color color = (Hovering | Selected) ? Color.Red : Color.Black;
            UIGraphic.DrawBorderText(spriteBatch,
                UIGraphic.FiascoFontSmall,
                _text,
                actualCoords.X + actualCoords.Width / 2 - _textOffset.X,
                actualCoords.Y + actualCoords.Height + 6,
                2.0f,
                color);

            spriteBatch.End();
        }

        private Texture2D _texture;
        private Rectangle _sprite;
        private string _text;
        private Vector2 _textOffset;
        private Matrix _transformation;
    }
}
