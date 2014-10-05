using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls.Coordinates
{
    /// <summary>
    /// Representation of a rectangle with relative coordinates.
    /// </summary>
    public struct RelativeRectangle
    {
        /// <summary>
        /// Relative vector representing the initial X position of this rectangle.
        /// </summary>
        public RelativeVector X { get { return _x; } }

        /// <summary>
        /// Relative vector representing the initial Y position of this rectangle.
        /// </summary>
        public RelativeVector Y { get { return _y; } }

        /// <summary>
        /// Float representing the width of this rectangle.
        /// </summary>
        public float Width { get { return _width; } }

        /// <summary>
        /// Float representing the height of this rectangle.
        /// </summary>
        public float Height { get { return _height; } }

        private RelativeVector _x, _y;
        private float _width, _height;

        /// <summary>
        /// Creates a new relative rectangle with the specified coordinates.
        /// </summary>
        /// <param name="X">Relative vector representing the initial X position of this rectangle.</param>
        /// <param name="Y">Relative vector representing the initial Y position of this rectangle.</param>
        /// <param name="width">Float representing the width of this rectangle.</param>
        /// <param name="height">Float representing the height of this rectangle.</param>
        public RelativeRectangle(RelativeVector X, RelativeVector Y, float width, float height)
        {
            this._x = X;
            this._y = Y;
            this._width = width;
            this._height = height;
        }

    }
}
