using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Etc.WeightedRandom
{
    /// <summary>
    /// Weighted random object generator.
    /// </summary>
    class RandomGenerator
    {
        public static Random Rand;

        public static WeightedRandom Generate(WeightedRandom[] list, int level)
        {
            int max = 0;
            List<WeightedRandom> table = new List<WeightedRandom>();

            // Add all items to table and determine maximum roll.
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].MinLevel <= level && list[i].MaxLevel >= level)
                {
                    table.Add(list[i]);
                    max += list[i].Weight;
                }
            }

            if (table.Count == 0)
                return null;

            int roll = Rand.Next(max);

            /*
             * Iterate through the list and check if the roll is less than each
             * item's weight. If it is not, subtract that item's weight from the
             * value of roll and move on to the next item.
             */
            for (int i = 0; i < table.Count; i++)
            {
                if (roll < table[i].Weight)
                    return table[i];

                roll -= table[i].Weight;
            }

            return null;
        }
    }
}
