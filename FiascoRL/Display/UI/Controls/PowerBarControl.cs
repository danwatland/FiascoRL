using FiascoRL.Display.UI.Controls.Coordinates;
using FiascoRL.Entities.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    /// <summary>
    /// Visual representation of a current/max value, i.e. HP. Displays a bar that fills up
    /// based on the percentage of a value compared to the maximum value set.
    /// </summary>
    public class PowerBarControl : Control
    {
        /// <summary>
        /// Creates a new powerbar control in the default position.
        /// </summary>
        /// <param name="color">Color of this powerbar control.</param>
        /// <param name="stat">Stat to be represented.</param>
        /// <param name="label">Label to be displayed.</param>
        public PowerBarControl(BarColor color, Stat stat, string label)
        {
            this.BarFillColor = color;
            this.Stat = stat;
            this._labelText = label;
            this._textDimensions = UIGraphic.FiascoFont.MeasureString(label) * 1.4f;
            this.WBitmap = new Rectangle(259, 38, 24, 24);
            this.CenterBitmap = new Rectangle(284, 38, 23, 24);
            this.EBitmap = new Rectangle(308, 38, 24, 24);
            RelativeRectangle coords = this.Coords = new RelativeRectangle(new RelativeVector(0.04f, 0.0f), 
                new RelativeVector(0.04f, 0.0f), 0.12f, 0.01f);
        }

        /// <summary>
        /// Creates a new powerbar control at the specified position.
        /// </summary>
        /// <param name="color">Color of this powerbar control.</param>
        /// <param name="stat">Stat to be represented.</param>
        /// <param name="coords">Coordinates of this powerbar control.</param>
        /// <param name="label">Label to be displayed.</param>
        public PowerBarControl(BarColor color, Stat stat, RelativeRectangle coords, string label)
            : this(color, stat, label)
        {
            this.Coords = coords;
        }

        /// <summary>
        /// Text to be displayed by this powerbar.
        /// </summary>
        public string LabelText
        {
            get
            {
                if (_labelText == null)
                {
                    _labelText = string.Empty;
                }
                return _labelText;
            }
            set
            {
                _labelText = value;
            }
        }

        private string _labelText;
       
        #region Bar Colors
        /// <summary>
        /// Enumeration of colors for a powerbar.
        /// </summary>
        public enum BarColor
        {
            Red,
            Yellow,
            Turquoise,
            Blue,
            DarkGreen,
            Green
        }
        #endregion

        /// <summary>
        /// Color of this Power Bar.
        /// </summary>
        public BarColor BarFillColor { get; set; }

        /// <summary>
        /// Current and maximum value of this power bar.
        /// </summary>
        public Stat Stat { get; set; }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Rectangle rect = GetActualCoords();
            Point barCoords = ConvertColorToCoords(this.BarFillColor);
            double percentFilled = ((double)Stat.Current / (double)Stat.Max);

            /// Draw fill.
            spriteBatch.Draw(UITexture, new Rectangle(rect.X + 8, rect.Y + 4, 8, 16), new Rectangle(barCoords.X, barCoords.Y, 8, 16), Color.White); // W portion
            spriteBatch.Draw(UITexture, new Rectangle(rect.X + 16, rect.Y + 4, (int)((rect.Width - 32) * percentFilled), 16), new Rectangle(barCoords.X + 9, barCoords.Y, 7, 16), Color.White); // C portion
            spriteBatch.Draw(UITexture, new Rectangle(rect.X + 16 + (int)((rect.Width - 32) * percentFilled), rect.Y + 4, 8, 16), new Rectangle(barCoords.X + 17, barCoords.Y, 8, 16), Color.White); // W portion

            UIGraphic.DrawBorderText(spriteBatch, UIGraphic.FiascoFont, LabelText, rect.X - _textDimensions.X - 4, rect.Y + _textDimensions.Y - 4, 1.4f);

        }

        #region Bar Color Coordinate Converter
        /// <summary>
        /// Returns first instance of graphic for this bar color.
        /// </summary>
        /// <param name="color">Bar color.</param>
        /// <returns>first instance of graphic for this bar color.</returns>
        private Point ConvertColorToCoords(BarColor color)
        {
            switch (color)
            {
                case BarColor.Red:
                    return new Point(341, 39);
                case BarColor.Yellow:
                    return new Point(370, 39);
                case BarColor.Turquoise:
                    return new Point(341, 55);
                case BarColor.Blue:
                    return new Point(370, 55);
                case BarColor.DarkGreen:
                    return new Point(341, 71);
                case BarColor.Green:
                    return new Point(370, 71);
                default:
                    return Point.Zero;
            }
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            // Nothing.
        }

        private Vector2 _textDimensions;
    }
}
