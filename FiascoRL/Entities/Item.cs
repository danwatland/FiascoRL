using FiascoRL.Entities.Util;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Entities
{
    /// <summary>
    /// An object that can be picked up and used in some way by either
    /// the player or creatures.
    /// </summary>
    public class Item : Actor, IEquatable<Item>
    {
        /// <summary>
        /// Create a new item.
        /// </summary>
        public Item() : base()
        {
            this.Texture = Display.SpriteGraphic.Items;
        }

        /// <summary>
        /// Categories of items.
        /// </summary>
        public enum ItemCategory
        {
            All,
            Weapon,
            Armor,
            Shield,
            Belt,
            Boots,
            Helmet,
            Necklace,
            Ring,
            Potion
        }

        /// <summary>
        /// Category this item falls into.
        /// </summary>
        public ItemCategory Category { get; set; }

        /// <summary>
        /// Name for this item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Quantity of this item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Create a new item matching the specified item type.
        /// </summary>
        /// <param name="type">Type of item.</param>
        /// <returns>A new item matching the item type.</returns>
        public static Item GenerateItem(ItemType type, int quantity = 1)
        {
            Item item = new Item();
            item.GraphicIndex = type.GraphicIndex;
            item.Name = type.Name;
            item.Quantity = quantity;
            item.Category = type.Category;
            return item;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            /// Do nothing for now.
        }

        public bool Equals(Item other)
        {
            return this.Name == other.Name &&
                this.GraphicIndex == other.GraphicIndex &&
                this.Texture == other.Texture;
        }
    }
}
