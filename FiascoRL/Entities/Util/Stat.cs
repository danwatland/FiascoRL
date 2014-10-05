using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Entities.Util
{
    public class Stat
    { 
        /// <summary>
        /// Creates a new stat with the specified current and max values.
        /// </summary>
        /// <param name="current">Current value of this stat.</param>
        /// <param name="max">Maximum value of this stat.</param>
        public Stat(int current, int max)
        {
            this.Current = current;
            this.Max = max;
        }

        /// <summary>
        /// Current value of this stat.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Value added from items, buffs, etc.
        /// </summary>
        public int Additional { get; set; }

        /// <summary>
        /// Maximum value of this stat.
        /// </summary>
        public int Max { get; set; }
    }
}
