using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Etc.WeightedRandom
{
    public interface WeightedRandom
    {
        /// <summary>
        /// Minimum level this element can appear on.
        /// </summary>
        int MinLevel { get; set; }

        /// <summary>
        /// Maximum level this element can appear on.
        /// </summary>
        int MaxLevel { get; set; }

        /// <summary>
        /// Weighted probability that this element will appear.
        /// </summary>
        int Weight { get; set; }

        /// <summary>
        /// Type specific to this particular object.
        /// </summary>
        Object Type { get; set; }
    }
}
