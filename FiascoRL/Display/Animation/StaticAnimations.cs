using FiascoRL.Display.UI;
using FiascoRL.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.Animation
{
    public static class StaticAnimations
    {
        public static TextAnimation DamageTextAnimation(Entity entity, int number) {
            TextAnimation animation = new TextAnimation(entity)
            {
                FrameLength = 0.025f,
                Loop = false,
                Text = number.ToString(),
                Font = UIGraphic.FiascoFont,
            };

            Vector2 offset = animation.Font.MeasureString(number.ToString());
            
            // Initial focus.
            for (int i = 6; i <= 16; i++)
            {
                animation.Frames.Add(new Frame()
                {
                    Color = Color.Red,
                    BorderColor = Color.White,
                    Text = number.ToString(),
                    Offset = new Point((int)(12 - (offset.X / 2.0) * ((float)Math.Pow((i / 4.0) - 4, 2) + 1)),
                                       (int)(12 - (offset.Y / 2.0) * ((float)Math.Pow((i / 4.0) - 4, 2) + 1))),
                    Size = (float)Math.Pow((i / 4.0) - 4, 2) + 1,
                });
            }

            // Float up and fade out.
            for (int i = 1; i < 10; i++)
            {
                animation.Frames.Add(new Frame()
                {
                    Color = new Color(1.0f, 0.0f, 0.0f, 1.0f - i * 0.1f),
                    BorderColor = new Color(1.0f, 1.0f, 1.0f, 1.0f - i * 0.1f),
                    Text = number.ToString(),
                    Offset = new Point((int)(12 - offset.X / 2.0), (int)(12 - offset.Y / 2.0) - i * 1),
                    Size = 1.0f,
                });
            }

            return animation;
        }
    }
}
