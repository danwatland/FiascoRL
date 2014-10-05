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
        private readonly Entity _entity;

        /// <summary>
        /// Creates a new stand-alone animation.
        /// </summary>
        private Animation()
        {
            this.Frames = new List<Frame>();
            this.Offset = new Point(0, 0);
        }

        /// <summary>
        /// Creates a new animation with the specified number of frames.
        /// </summary>
        /// <param name="frames">Number of frames.</param>
        public Animation(int frames) : this()
        {
            Frames = (new Frame()).CreateClonedEnumerable(frames).ToList();
            _currentFrame = Frames[0];
        }

        /// <summary>
        /// Creates an animation tied to a particular entity that lasts for the specified number of frames.
        /// </summary>
        /// <param name="e">Entity to tie animation to.</param>
        /// <param name="frames">Number of frames.</param>
        public Animation(Entity e, int frames) : this()
        {
            this._entity = e;
            Frames = (new Frame()).CreateClonedEnumerable(frames).ToList();
            _currentFrame = Frames[0];
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
        public int GraphicIndex
        {
            get
            {
                if (_entity == null)
                    return Frames.Any() ? _currentFrame.GraphicIndex : 0;
                else
                    return _entity.GraphicIndex;
            }
        }

        /// <summary>
        /// Returns coordinates of either the entity tied to this animation 
        /// (if an entity is tied to it) or the animation itself.
        /// </summary>
        public Point Coords { 
            get 
            {
                if (_entity != null && _entity.GetType().IsSubclassOf(typeof (Actor)))
                {
                    return ((Creature)_entity).Coords;
                }

                else
                {
                    return _coords;
                } 
            } 
            set { _coords = value; } 
        }
        private Point _coords;

        /// <summary>
        /// Whether or not this entity's animation repeats indefinately.
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// List representing all frames for this entity's animation.
        /// </summary>
        public List<Frame> Frames { get; set; }

        /// <summary>
        /// Color of the frame being shown.
        /// </summary>
        public Color CurrentColor
        {
            get { return _currentFrame.Color; }
        }

        /// <summary>
        /// Length of each frame in this entity's animation.
        /// </summary>
        public float FrameLength { get; set; }

        /// <summary>
        /// Elapsed time since this entity has updated.
        /// </summary>
        protected float _frameTimer { get; set; }

        /// <summary>
        /// Current frame this entity is displaying.
        /// </summary>
        protected Frame _currentFrame { get; set; }

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
                _currentFrame = Frames[(Frames.IndexOf(_currentFrame) + 1) % Math.Max(Frames.Count, 1)];

                if ((_currentFrame.Equals(Frames[0]) && Frames.Count > 0) && !Loop)
                {
                    Completed = true;
                }
            }
        }

        public object Clone()
        {
            var clone = new Animation()
            {
                Frames = new List<Frame>(this.Frames.Select(x => (Frame)x.Clone())),
                _currentFrame = this._currentFrame,
                Completed = this.Completed,
                Coords = this.Coords,
                FrameLength = this.FrameLength,
                Loop = this.Loop,
                Offset = this.Offset,
                Texture = this.Texture,
            };

            return clone;
        }
    }
}
