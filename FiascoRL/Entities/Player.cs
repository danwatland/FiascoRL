using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Etc;
using FiascoRL.Display;
using FiascoRL.Etc.Targeting;
using Microsoft.Xna.Framework;

namespace FiascoRL.Entities
{
    public class Player : Creature
    {
        /// <summary>
        /// Current game turn.
        /// </summary>
        public long CurrentTurn { get; set; }

        /// <summary>
        /// Player's line of sight.
        /// </summary>
        public LOS LOS { get; set; }

        public Player(Texture2D texture, int graphicIndex)
            : base(texture, graphicIndex)
        {
            this.LOS = new LOS(this);
            this.baseRegenSpeed = 6;
            this.Items.Add(new Item() { Name = "Gold", Quantity = 240, Texture = SpriteGraphic.Items, GraphicIndex = 74, Category = Item.ItemCategory.All});
            this.Items.Add(new Item() { Name = "Potion", Quantity = 1, Texture = SpriteGraphic.Items, GraphicIndex = 0, Category = Item.ItemCategory.Potion });
            this.Items.Add(new Item() { Name = "Sword", Quantity = 1, Texture = SpriteGraphic.Items, GraphicIndex = 74, Category = Item.ItemCategory.Weapon });
            this.Items.Add(new Item() { Name = "Shield", Quantity = 1, Texture = SpriteGraphic.Items, GraphicIndex = 74, Category = Item.ItemCategory.Shield });
            this.Name = "Brimley";
            this.Target = new Target(this);
        }

        /// <summary>
        /// Targeting area for this player.
        /// </summary>
        public Target Target { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Target.Update(gameTime);
        }
    }
}
