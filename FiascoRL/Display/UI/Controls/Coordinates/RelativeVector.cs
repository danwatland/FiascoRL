using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiascoRL.Display.UI.Controls.Coordinates
{
    /// <summary>
    /// Representation of a vector with a relative position and offset.
    /// </summary>
    public struct RelativeVector
    {
        /// <summary>
        /// Initial position (from 0.0 to 1.0).
        /// </summary>
        public float Position { get { return MathHelper.Clamp(_position, 0.0f, 1.0f); } set { _position = value; } }

        private float _position;

        /// <summary>
        /// Offset from original position.
        /// </summary>
        public float Offset { get { return _offset; } set { _offset = value; } }

        private float _offset;

        /// <summary>
        /// Creates a new relative vector with the specified offset from the initial position.
        /// </summary>
        /// <param name="position">Float representing percentage of available area.</param>
        /// <param name="offset">Offset from initial position.</param>
        public RelativeVector(float position, float offset)
        {
            this._position = position;
            this._offset = offset;
        }
    }
}
