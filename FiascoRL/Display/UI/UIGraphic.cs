﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FiascoRL.Display.UI
{
    public static class UIGraphic
    {
        public static SpriteFont FiascoFont { get; private set; }
        public static SpriteFont FiascoFontSmall { get; private set; }

        public static void Initialize(FiascoGame game)
        {
            FiascoFont = game.Content.Load<SpriteFont>("fiasco_font");
            FiascoFont.Spacing = 1.0f;
            FiascoFontSmall = game.Content.Load<SpriteFont>("fiasco_font_small");
            FiascoFontSmall.Spacing = 1.0f;
        }

        /// <summary>
        /// Draws text with a one pixel black border around it.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="text">Text to display.</param>
        /// <param name="xPos">X-coordinate of text.</param>
        /// <param name="yPos">Y-coordinate of text.</param>
        public static void DrawBorderText(SpriteBatch spriteBatch, SpriteFont font, string text, float xPos, float yPos, float size)
        {
            DrawBorderText(spriteBatch, font, text, xPos, yPos, size, Color.Black);
        }

        /// <summary>
        /// Draws text with a one pixel color border around it.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="text">Text to display.</param>
        /// <param name="xPos">X-coordinate of text.</param>
        /// <param name="yPos">Y-coordinate of text.</param>
        /// <param name="borderColor">Color of text border.</param>
        public static void DrawBorderText(SpriteBatch spriteBatch, SpriteFont font, string text, float xPos, float yPos, float size, Color borderColor)
        {
            DrawBorderText(spriteBatch, font, text, xPos, yPos, size, borderColor, Color.White);
        }

        /// <summary>
        /// Draws color text with a one pixel color border around it.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="font">SpriteFont to use.</param>
        /// <param name="text">Text to display.</param>
        /// <param name="xPos">X-coordinate of text.</param>
        /// <param name="yPos">Y-coordinate of text.</param>
        /// <param name="size">Size of text.</param>
        /// <param name="borderColor">Color of text border.</param>
        /// <param name="textColor">Color of text.</param>
        public static void DrawBorderText(SpriteBatch spriteBatch, SpriteFont font, string text, float xPos, float yPos, float size, Color borderColor, Color textColor)
        {
            spriteBatch.DrawString(font, text, new Vector2(xPos + 1, yPos + 1), borderColor, 0.0f, Vector2.Zero, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, text, new Vector2(xPos - 1, yPos + 1), borderColor, 0.0f, Vector2.Zero, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, text, new Vector2(xPos - 1, yPos - 1), borderColor, 0.0f, Vector2.Zero, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, text, new Vector2(xPos + 1, yPos - 1), borderColor, 0.0f, Vector2.Zero, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, text, new Vector2(xPos, yPos), textColor, 0.0f, Vector2.Zero, size, SpriteEffects.None, 0.0f);
        }

        /// <summary>
        /// Draws the specified portion of a texture with a one pixel border around it.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to.</param>
        /// <param name="texture">Texture to pull image from.</param>
        /// <param name="destRect">Where image should be displayed on-screen.</param>
        /// <param name="sourceRect">Source of image from texture.</param>
        /// <param name="borderColor">Color of border.</param>
        public static void DrawBorder(SpriteBatch spriteBatch, Texture2D texture, Rectangle destRect, Rectangle sourceRect, Color originalColor, Color borderColor)
        {
            spriteBatch.Draw(texture, new Rectangle(destRect.X + 1, destRect.Y + 1, destRect.Width, destRect.Height), sourceRect, borderColor);
            spriteBatch.Draw(texture, new Rectangle(destRect.X + 1, destRect.Y - 1, destRect.Width, destRect.Height), sourceRect, borderColor);
            spriteBatch.Draw(texture, new Rectangle(destRect.X - 1, destRect.Y + 1, destRect.Width, destRect.Height), sourceRect, borderColor);
            spriteBatch.Draw(texture, new Rectangle(destRect.X - 1, destRect.Y - 1, destRect.Width, destRect.Height), sourceRect, borderColor);
            spriteBatch.Draw(texture, destRect, sourceRect, originalColor);
        }
    }
}
