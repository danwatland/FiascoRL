using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.World
{
    public class Staircase : Tile
    {
        public Staircase(int graphicIndex)
            : base(graphicIndex, true)
        {

        }

        /// <summary>
        /// X and Y coordinates of this staircase.
        /// </summary>
        public Point Coords { get; set; }

        /// <summary>
        /// Staircase connected to this one.
        /// </summary>
        public Staircase ConnectingStaircase { get; set; }

        /// <summary>
        /// Level this staircase is on.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        /// Connects this staircase to another.
        /// </summary>
        /// <param name="staircase">Staircase to connect to.</param>
        public void ConnectStaircase(Staircase staircase)
        {
            this.ConnectingStaircase = staircase;
            staircase.ConnectingStaircase = this;
        }

        public enum StairType
        {
            Down,
            Up
        }
    }
}
