using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiascoRL.Display
{
    class Camera2D
    {
        protected float _zoom; // Camera Zoom
        public Matrix _transform; // Matrix Transform
        public Vector2 _pos; // Camera Position
        protected float _rotation; // Camera Rotation

        /// <summary>
        /// Create a new default camera.
        /// </summary>
        public Camera2D()
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
        }

        /// <summary>
        /// Amount to zoom the images drawn to the screen.
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
        }

        /// <summary>
        /// Amount to rotate the images drawn to the screen.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Auxiliary function to move the camera.
        /// </summary>
        /// <param name="amount">2D Vector to move the camera by.</param>
        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        /// <summary>
        /// Position of this camera.
        /// </summary>
        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        
        /// <summary>
        /// Returns the transformation matrix for this camera.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice to perform transformation on.</param>
        /// <returns>Transformation matrix for this camera.</returns>
        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            _transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return _transform;
        }
    }
}
