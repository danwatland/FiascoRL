using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Entities.Util
{
    /// <summary>
    /// Class containing pre-defined item types and variables.
    /// </summary>
    public class ItemType : ObjectType
    {
        public static ItemType GOLD = new ItemType("Gold", 74, Item.ItemCategory.Armor);
        public static ItemType POTION = new ItemType("Potion", 0, Item.ItemCategory.Potion);

        ItemType(string name, int graphicIndex, Item.ItemCategory category)
        {
            this.Name = name;
            this.GraphicIndex = graphicIndex;
            this.Category = category;
        }

        public String Name { get; set; }

        public int Quantity { get; set; }

        public int GraphicIndex { get; set; }

        public Item.ItemCategory Category { get; set; }
    }
}
