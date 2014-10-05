using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NewRL.Entities;

namespace NewRL.Display
{
    /// <summary>
    /// Collection of utility methods and variables used for processing graphics drawn to the screen.
    /// </summary>
    public class SpriteGraphic
    {
        public static Texture2D Icons { get; private set; }

        public static Texture2D Effects { get; private set; }

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
            Effects = game.Content.Load<Texture2D>("oryx_16bit_fantasy_fx_trans");
            Items = game.Content.Load<Texture2D>("oryx_16bit_fantasy_items_trans");
            World = game.Content.Load<Texture2D>("oryx_16bit_fantasy_world_trans");
            Creatures = game.Content.Load<Texture2D>("oryx_16bit_fantasy_creatures_trans");
        }

        /// <summary>
        /// Return a rectangle representing the specified sprite from a 24x24 spritesheet.
        /// </summary>
        /// <param name="texture">Spritesheet to be used.</param>
        /// <param name="index">Index of sprite.</param>
        /// <returns></returns>
        public static Rectangle Get24By24(Texture2D texture, int index)
        {
            int numColumns = texture.Width / 24 - 2;
            // World spritesheet does not line up exactly.
            if (texture == World) numColumns++;
            int row = 1 + index / numColumns;
            int col = 1 + index % numColumns;
            return new Rectangle(col * 24, row * 24, 24, 24);
        }

        /// <summary>
        /// Draws healthbars for each creature.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="c"></param>
        public static void DrawHealthBar(SpriteBatch spriteBatch, Game game, Creature c)
        {
            Texture2D texture = new Texture2D(game.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            if (c.HPCurrent < c.HPMax)
            {
                int length = (int)(((double)c.HPCurrent / c.HPMax) * 24);
                spriteBatch.Draw(texture, new Rectangle(c.Coords.X * 24, c.Coords.Y * 24 + 22, 24, 2), Color.Red);
                spriteBatch.Draw(texture, new Rectangle(c.Coords.X * 24, c.Coords.Y * 24 + 22, length, 2), Color.Lime);
            }

        }
    }
}
