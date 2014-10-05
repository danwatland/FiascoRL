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
            Spell fireball = new Spell(performer, recipient) { Damage = 3, Name = "Fireball", SkillPoints = 2 };
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
                //Colors = (new Color[] {Color.OrangeRed , Color.OrangeRed, Color.OrangeRed, Color.OrangeRed, Color.OrangeRed, 
//                    Color.OrangeRed, Color.OrangeRed, Color.OrangeRed, Color.OrangeRed, Color.OrangeRed, Color.OrangeRed, Color.OrangeRed,
//                    new Color(1.0f, 0.0f, 0.0f, 0.8f), new Color(1.0f, 0.0f, 0.0f, 0.6f),
//                    new Color(1.0f, 0.0f, 0.0f, 0.4f), new Color(1.0f, 0.0f, 0.0f, 0.2f)}).ToList(),


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
//                Frames = (new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }).ToList(),
//                Colors = (new Color[] {Color.White, Color.White, Color.White, Color.White, Color.White, Color.White, 
//                    Color.White, Color.White, Color.White, Color.White, Color.Transparent, Color.Transparent,
//                    Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent}).ToList(),

            var anim3 = (Animation)anim2.Clone();
            anim3.SetGraphicIndexes(new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4 });
            anim3.SetColors(Color.Transparent);
            anim3.SetColors(Color.White, 6, 9);
//            anim3.Frames = (new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4 }).ToList();
//            anim3.Colors = (new Color[] {Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, 
//                Color.Transparent, Color.Transparent, Color.White, Color.White, Color.White, Color.White,
//                Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent, Color.Transparent}).ToList();

            fireball.Animations.Add(anim);
            fireball.Animations.Add(anim2);
            fireball.Animations.Add(anim3);

            return fireball;
        }

        private Spell(Creature performer, Creature recipient)
        {
            this.Performer = performer;
            this.Recipient = recipient;
            this.TargetCoords = recipient.Coords;
            this._rand = new Random();
        }

        protected int Damage { get; private set; }

        protected override void PerformSkill(Creature performer, Creature recipient)
        {
            recipient.HP.Current -= Damage;
            Session.Animations.Add(recipient.DamageAnimation());
            Session.MessageLog.Enqueue(String.Format("{0} casts {1} on {2} for {3} damage.", performer.Name, this.Name, recipient.Name, this.Damage));
            Session.TextAnimations.Add(StaticAnimations.DamageTextAnimation(recipient, Damage));
        }

        private Random _rand;
    }
}
