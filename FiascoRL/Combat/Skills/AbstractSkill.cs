using FiascoRL.Display;
using FiascoRL.Display.Animation;
using FiascoRL.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Combat.Skills
{
    public abstract class AbstractSkill
    {
        /// <summary>
        /// Key this skill is bound to.
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// Name of this skill.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Number of skill points it takes to use this skill.
        /// </summary>
        public int SkillPoints { get; set; }

        /// <summary>
        /// Animations displayed when this action is performed.
        /// </summary>
        public List<Animation> Animations
        {
            get
            {
                if (_animations == null)
                {
                    _animations = new List<Animation>();
                }

                return _animations;
            }
            set
            {
                _animations = value;
            }
        }

        public Point TargetCoords { get; set; }

        /// <summary>
        /// Performs this skill.
        /// </summary>
        /// <param name="performer">Creature performing this skill.</param>
        /// <param name="recipient">Creature this skill is being performed on.</param>
        public void Perform(Creature performer, Creature recipient)
        {
            performer.SP.Current -= SkillPoints;
            Reset();
            PerformSkill(performer, recipient);
            Animations.ForEach(x =>
            {
                if (!Session.Animations.Contains(x))
                {
                    Session.Animations.Add(x);
                }
            });
        }

        protected abstract void PerformSkill(Creature performer, Creature recipient);

        protected Creature Performer { get; set; }
        protected Creature Recipient { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Animation animation in Animations)
            {
                animation.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            foreach (Animation animation in Animations) 
            {
                Session.SpriteBatch.Draw(animation.Texture,
                    new Rectangle((animation.Coords.X) * 24 + animation.Offset.X,
                        (animation.Coords.Y) * 24 + animation.Offset.Y, 24, 24),
                    SpriteGraphic.GetSprite(animation.Texture, animation.GraphicIndex),
                    animation.CurrentColor);

            }

            Session.SpriteBatch.End();
        }

        private void Reset()
        {
            Animations.ForEach(x => x.Completed = false);
        }

        private List<Animation> _animations;
    }
}
