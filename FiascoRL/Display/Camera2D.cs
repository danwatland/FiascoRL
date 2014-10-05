using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiascoRL.Display
{
    public class Camera2D
    {
        /// <summary>
        /// Create a new default camera.
        /// </summary>
        public Camera2D()
        {
            _zoom = 1.0f;
            Rotation = 0.0f;
            Pos = Vector2.Zero;
        }

        /// <summary>
        /// Amount to zoom the images drawn to the screen.
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set  // Negative zoom will flip image
            { 
                _zoom = value; 
                if (_zoom < 0.1f) _zoom = 0.1f; 
            }
        }
        private float _zoom;

        /// <summary>
        /// Amount to rotate the images drawn to the screen.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Position of this camera.
        /// </summary>
        public Vector2 Pos { get; set; }

        /// <summary>
        /// Auxiliary function to move the camera.
        /// </summary>
        /// <param name="amount">2D Vector to move the camera by.</param>
        public void Move(Vector2 amount)
        {
            Pos += amount;
        }

        
        /// <summary>
        /// Returns the transformation matrix for this camera.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice to perform transformation on.</param>
        /// <returns>Transformation matrix for this camera.</returns>
        public Matrix get_transformation(GraphicsDevice graphicsDevice)
        {
            // Thanks to o KB o for this solution
            var transform = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return transform;
        }
    }
}
