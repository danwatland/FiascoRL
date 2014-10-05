using System.Net.Mail;
using FiascoRL.Display;
using FiascoRL.Display.Animation;
using FiascoRL.Display.UI;
using FiascoRL.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Combat.Skills
{
    public class Spell : AbstractSkill
    {

        public static Spell Fireball(Creature performer, Creature recipient)
        {
            var fireball = new Spell() { Damage = 3, Name = "Fireball", SkillPoints = 2 };
            // Initial fireball.
            var anim = new Animation(16)
            {
                FrameLength = 0.02f,
                Texture = SpriteGraphic.Effects32,
                Coords = recipient.Coords,
            };
            anim.SetGraphicIndexes(new int[] { 5, 5, 5, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 9, 9, 9 });
            anim.SetColors(Color.OrangeRed);
            anim.AddLinearFade(12, 15);

            // Explosion expanding...
            var anim2 = new Animation(16)
            {
                FrameLength = 0.02f,
                Texture = SpriteGraphic.Effects32,
                Coords = recipient.Coords,
            };
            anim2.SetGraphicIndexes(2);
            anim2.SetColors(Color.White);
            anim2.SetColors(Color.Transparent, 11, 15);

            var anim3 = (Animation)anim2.Clone();
            anim3.SetGraphicIndexes(new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4 });
            anim3.SetColors(Color.Transparent);
            anim3.SetColors(Color.White, 6, 9);

            fireball.Animations.Add(anim);
            fireball.Animations.Add(anim2);
            fireball.Animations.Add(anim3);
            fireball.Skill = delegate(Creature p, Creature r)
            {
                r.HP.Current -= fireball.Damage;
                Session.Animations.Add(r.DamageAnimation());
                Session.MessageLog.Enqueue(String.Format("{0} casts {1} on {2} for {3} damage.", p.Name, fireball.Name, r.Name, fireball.Damage));
                Session.TextAnimations.Add(StaticAnimations.DamageTextAnimation(r, fireball.Damage));
            };

            return fireball;
        }

        private Spell()
        {
            this._rand = new Random();
        }

        protected int Damage { get; private set; }

        protected override void PerformSkill(Creature performer, Creature recipient)
        {
            Skill.Invoke(performer, recipient);
        }

        private Random _rand;
    }
}
