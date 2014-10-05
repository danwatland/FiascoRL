using FiascoRL.Display.UI.Controls.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class WindowButtonControl : Control
    {
        #region Properties and enums
        private Rectangle _overlayRectangle { get; set; }
        private Rectangle _baseButtonRectangle { get; set; }
        private OverlayType _overlayType;

        public enum OverlayType
        {
            Minus,
            Plus,
            Close
        }

        public enum ButtonColor
        {
            Green,
            Blue,
            Red,
            Gray
        }
        #endregion

        public WindowButtonControl(ButtonColor buttonColor, OverlayType overlayType)
        {
            this._overlayType = overlayType;
            this.CenterBitmap = ButtonColorToRect(buttonColor);
            this._overlayRectangle = OverlayTypeToRect(overlayType);
            this._baseButtonRectangle = CenterBitmap;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Rectangle actual = GetActualCoords();
            spriteBatch.Draw(UITexture, new Rectangle(actual.X, actual.Y, 16, 16), _overlayRectangle, Color.White);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Parent == null)
                return;

            MouseState ms = Mouse.GetState();
            Rectangle actualCoords = GetActualCoords();
            Point currentMousePos = new Point(ms.X, ms.Y);

            if (ms.LeftButton == ButtonState.Pressed && actualCoords.Contains(currentMousePos))
            {
                CenterBitmap = new Rectangle(_baseButtonRectangle.X, _baseButtonRectangle.Y + 32, 16, 16);
                if (!Parent.ButtonPressed)
                {
                    ((IButtonHandler)Parent).ButtonHandler(_overlayType);
                }
                Parent.ButtonPressed = true;
            }
            else if (actualCoords.Contains(currentMousePos))
            {
                CenterBitmap = new Rectangle(_baseButtonRectangle.X, _baseButtonRectangle.Y + 16, 16, 16);
                Parent.ButtonPressed = false;
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                CenterBitmap = _baseButtonRectangle;
                Parent.ButtonPressed = false;
            }
        }

        #region Helper methods
        private Rectangle OverlayTypeToRect(OverlayType type)
        {
            switch (type)
            {
                case OverlayType.Minus:
                    return new Rectangle(96, 120, 16, 16);
                case OverlayType.Plus:
                    return new Rectangle(80, 120, 16, 16);
                case OverlayType.Close:
                    return new Rectangle(96, 136, 16, 16);
                default:
                    return default(Rectangle);
            }
        }

        private Rectangle ButtonColorToRect(ButtonColor color)
        {
            switch (color)
            {
                case ButtonColor.Green:
                    return new Rectangle(15, 104, 16, 16);
                case ButtonColor.Blue:
                    return new Rectangle(31, 104, 16, 16);
                case ButtonColor.Red:
                    return new Rectangle(47, 104, 16, 16);
                case ButtonColor.Gray:
                    return new Rectangle(63, 104, 16, 16);
                default:
                    return default(Rectangle);
            }
        }

        #endregion

        
    }
}
