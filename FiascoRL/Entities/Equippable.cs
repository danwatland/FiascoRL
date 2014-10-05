using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Entities
{
    public class Equippable : Item
    {
        public Equippable()
            : base()
        {

        }

        /// <summary>
        /// Amount of attack this item adds.
        /// </summary>
        public int Attack { get; set; }

        /// <summary>
        /// Amount of defense this item adds.
        /// </summary>
        public int Defense { get; set; }

        /// <summary>
        /// Amount of strength this item adds.
        /// </summary>
        public int Strength { get; set; }

        /// <summary>
        /// Amount of intelligence this item adds.
        /// </summary>
        public int Intelligence { get; set; }

        /// <summary>
        /// Amount of HP this item adds.
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// Amount of MP this item adds.
        /// </summary>
        public int MP { get; set; }

        /// <summary>
        /// Amount of agility this item adds.
        /// </summary>
        public int Agility { get; set; }
    }
}
