using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Entities.Util
{
    public class EquippedItems
    {
        public EquippedItems()
        {

        }

        public Equippable Weapon1 { get; set; }
        public Equippable Weapon2 { get; set; }
        public Equippable Armor { get; set; }
        public Equippable Shield { get; set; }
        public Equippable Helmet { get; set; }
        public Equippable Gloves { get; set; }
        public Equippable Boots { get; set; }
        public Equippable Ring1 { get; set; }
        public Equippable Ring2 { get; set; }
        public Equippable Amulet { get; set; }
    }
}
