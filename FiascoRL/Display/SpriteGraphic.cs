using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Entities;

namespace FiascoRL.Display
{
    /// <summary>
    /// Collection of utility methods and variables used for processing graphics drawn to the screen.
    /// </summary>
    public static class SpriteGraphic
    {
        public static Texture2D Icons { get; private set; }
        public static Texture2D Effects24 { get; private set; }
        public static Texture2D Effects32 { get; private set; }
        public static Texture2D Items { get; private set; }
        public static Texture2D World { get; private set; }
        public static Texture2D Creatures { get; private set; }

        /// <summary>
        /// Load all textures used by the game into memory.
        /// </summary>
        /// <param name="game">Game to load textures for.</param>
        public static void Initialize(Game game)
        {
            Icons = game.Content.Load<Texture2D>("oryx_16bit_fantasy_classes_trans");
            Effects24 = game.Content.Load<Texture2D>("oryx_16bit_fantasy_fx_trans");
            Effects32 = game.Content.Load<Texture2D>("oryx_16bit_fantasy_fx_trans");
            Items = game.Content.Load<Texture2D>("oryx_16bit_fantasy_items_trans");
            World = game.Content.Load<Texture2D>("oryx_16bit_fantasy_world_trans");
            Creatures = game.Content.Load<Texture2D>("oryx_16bit_fantasy_creatures_trans");
        }

        public static Rectangle GetSprite(Texture2D texture, int index)
        {
            if (texture == World || texture == Creatures || texture == Effects24)
            {
                return Get24By24(texture, index);
            }
            else if (texture == Items)
            {
                return Get16By16(texture, index);
            }
            else if (texture == Effects32)
            {
                return Get32By32Effect(texture, index);
            }
            else
            {
                throw new InvalidOperationException("Texture not recognized.");
            }
        }

        /// <summary>
        /// Draws healthbars for each creature.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="c"></param>
        public static void DrawHealthBar(SpriteBatch spriteBatch, FiascoGame game, Creature c)
        {
            var texture = new Texture2D(game.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            if (c.HP.Current < c.HP.Max)
            {
                int length = (int)(((double)c.HP.Current / c.HP.Max) * 24);
                spriteBatch.Draw(texture, new Rectangle(c.Coords.X * 24, c.Coords.Y * 24 + 22, 24, 2), Color.Red);
                spriteBatch.Draw(texture, new Rectangle(c.Coords.X * 24, c.Coords.Y * 24 + 22, length, 2), Color.Lime);
            }
        }

        /// <summary>
        /// Return a rectangle representing the specified sprite from a 24x24 spritesheet.
        /// </summary>
        /// <param name="texture">Spritesheet to be used.</param>
        /// <param name="index">Index of sprite.</param>
        /// <returns></returns>
        private static Rectangle Get24By24(Texture2D texture, int index)
        {
            int numColumns = texture.Width / 24 - 2;
            // World spritesheet does not line up exactly.
            if (texture == World)
            {
                numColumns++;
            }
            else if (texture == Effects24)
            {
                numColumns = 10;
            }
            int row = 1 + index / numColumns;
            int col = 1 + index % numColumns;
            return new Rectangle(col * 24, row * 24, 24, 24);
        }

        /// <summary>
        /// Return a rectangle representing the specified sprite from a 16x16 spritesheet.
        /// </summary>
        /// <param name="texture">Spritesheet to be used.</param>
        /// <param name="index">Index of sprite.</param>
        /// <returns></returns>
        private static Rectangle Get16By16(Texture2D texture, int index)
        {
            int numColumns = texture.Width / 16 - 2;
            int row = 1 + index / numColumns;
            int col = 1 + index % numColumns;
            return new Rectangle(col * 16, row * 16, 16, 16);
        }

        private static Rectangle Get32By32Effect(Texture2D texture, int index)
        {
            int row = index / 8;
            int col = index % 8;
            return new Rectangle(col * 32 + 287, row * 32 + 32, 32, 32);
        }
    }
}
