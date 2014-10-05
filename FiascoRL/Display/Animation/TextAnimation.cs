using FiascoRL.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.Animation
{
    public class TextAnimation
    {
        private Entity _entity;

        public TextAnimation(Entity entity) : this()
        {
            this._entity = entity;
        }


        public TextAnimation()
        {
            this.Frames = new List<Frame>();
        }

        /// <summary>
        /// Returns coordinates of either the entity tied to this animation 
        /// (if an entity is tied to it) or the animation itself.
        /// </summary>
        public Point Coords
        {
            get
            {
                if (_entity != null && _entity.GetType().IsSubclassOf(typeof(Actor)))
                {
                    return ((Creature)_entity).Coords;
                }
                else
                {
                    return _coords;
                }
            }
            set
            {
                _coords = value;
            }
        }

        private Point _coords;

        /// <summary>
        /// SpriteFont this animation uses.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Whether or not this animation repeats indefinately.
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// List of frames displayed in this animation.
        /// </summary>
        public List<Frame> Frames { get; set; }

        /// <summary>
        /// Text displayed in this animation.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Current color of this text.
        /// </summary>
        public Color TextColor { get; set; }

        /// <summary>
        /// Current color of this text's border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Current offset of this text from Coords.
        /// </summary>
        public Point Offset { get; set; }

        /// <summary>
        /// Current size of this text.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// Length of each frame in this animation.
        /// </summary>
        public float FrameLength { get; set; }

        /// <summary>
        /// Elapsed time since this animation has updated.
        /// </summary>
        private float _frameTimer { get; set; }

        /// <summary>
        /// Current color of text this animation is displaying.
        /// </summary>
        private int _currentFrame { get; set; }

        /// <summary>
        /// Whether or not this animation is completed.
        /// </summary>
        public bool Completed { get; set; }

        public void Update(GameTime gameTime)
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameTimer > FrameLength)
            {
                _frameTimer = 0.0f;
                _currentFrame = (_currentFrame + 1) % Math.Max(Frames.Count, 1);

                if (Frames.Count > 0)
                {
                    TextColor = Frames[_currentFrame].Color;
                    Text = Frames[_currentFrame].Text;
                    BorderColor = Frames[_currentFrame].BorderColor;
                    Offset = Frames[_currentFrame].Offset;
                    Size = Frames[_currentFrame].Size;
                }

                if ((_currentFrame == 0 && Frames.Count > 0) && !Loop)
                {
                    Completed = true;
                }
            }
        }
    }
}
