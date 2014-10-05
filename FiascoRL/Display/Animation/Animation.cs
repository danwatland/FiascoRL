using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Entities;

namespace FiascoRL.Display.Animation
{
    public class Animation : ICloneable
    {
        private Entity entity;

        /// <summary>
        /// Create an animation tied to a particular entity.
        /// </summary>
        /// <param name="e">Entity to tie animation to.</param>
        public Animation(Entity e) : this()
        {
            this.entity = e;
            this.GraphicIndex = e.GraphicIndex;
        }

        /// <summary>
        /// Creates a new stand-alone animation.
        /// </summary>
        public Animation()
        {
            this.Colors = new List<Color>();
            this.Frames = new List<int>();
            this.Offset = new Point(0, 0);
        }

        /// <summary>
        /// Texture this entity utilizes.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Offset (in pixels) for this animation's display.
        /// </summary>
        public Point Offset { get; set; }

        /// <summary>
        /// Index this entity uses from its spritesheet.
        /// </summary>
        public int GraphicIndex { get { if (entity != null && entity.GetType().IsSubclassOf(typeof(Actor))) { return ((Creature)entity).GraphicIndex; } 
                                        else { return _graphicIndex; } } set { _graphicIndex = value; } }

        private int _graphicIndex;

        /// <summary>
        /// Returns coordinates of either the entity tied to this animation 
        /// (if an entity is tied to it) or the animation itself.
        /// </summary>
        public Point Coords { get { if (entity != null && entity.GetType().IsSubclassOf(typeof(Actor))) { return ((Creature)entity).Coords; } 
                                    else { return _coords; } } set { _coords = value; } }

        private Point _coords;

        /// <summary>
        /// Whether or not this entity's animation repeats indefinately.
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// List representing all frames for this entity's animation.
        /// </summary>
        public List<Int32> Frames { get; set; }

        /// <summary>
        /// List of colors to draw this animation.
        /// </summary>
        public List<Color> Colors { get; set; }

        /// <summary>
        /// Color of the frame being shown.
        /// </summary>
        public Color CurrentColor { get; set; }

        /// <summary>
        /// Length of each frame in this entity's animation.
        /// </summary>
        public float FrameLength { get; set; }

        /// <summary>
        /// Elapsed time since this entity has updated.
        /// </summary>
        private float _frameTimer { get; set; }

        /// <summary>
        /// Current frame this entity is displaying.
        /// </summary>
        private int _currentFrame { get; set; }

        /// <summary>
        /// Current color frame this entity is displaying.
        /// </summary>
        private int _currentColorFrame { get; set; }

        /// <summary>
        /// Whether or not this entity's animation is completed.
        /// </summary>
        public bool Completed { get; set; }

        public void Update(GameTime gameTime)
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameTimer > FrameLength)
            {
                _frameTimer = 0.0f;
                _currentFrame = (_currentFrame + 1) % Math.Max(Frames.Count, 1);
                _currentColorFrame = (_currentColorFrame + 1) % Math.Max(Colors.Count, 1);

                if (Colors.Count > 0)
                {
                    CurrentColor = Colors[_currentColorFrame];
                }

                if (Frames.Count > 0)
                {
                    GraphicIndex = Frames[_currentFrame];
                }

                if (((_currentFrame == 0 && Frames.Count > 0) || (_currentColorFrame == 0 && Colors.Count > 0)) && !Loop)
                {
                    Completed = true;
                }
            }
        }

        public object Clone()
        {
            Animation clone = new Animation()
            {
                Colors = new List<Color>(this.Colors.Select(x => x)),
                Frames = new List<int>(this.Frames.Select(x => x)),
                CurrentColor = this.CurrentColor,
                Completed = this.Completed,
                Coords = this.Coords,
                FrameLength = this.FrameLength,
                GraphicIndex = this.GraphicIndex,
                Loop = this.Loop,
                Offset = this.Offset,
                Texture = this.Texture,
            };

            return clone;
        }
    }
}
