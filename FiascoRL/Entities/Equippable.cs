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

        public Action<Creature> OnEquip { get; set; }
        public Action<Creature> OnDeEquip { get; set; }
    }
}
