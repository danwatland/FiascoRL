using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Entities;
using FiascoRL.Display.Animation;

namespace FiascoRL.World
{
    /// <summary>
    /// Representation of an inanimate object in the environment.
    /// </summary>
    public class Tile : Entity
    {
        /// <summary>
        /// Whether or not this tile is traversable by actors.
        /// </summary>
        public bool Traversable { get; set; }

        /// <summary>
        /// Turn that the player last had this tile in line of sight.
        /// </summary>
        public long TurnSeen { get { return _turnSeen; } }

        /// <summary>
        /// Distance of this tile from the player.
        /// </summary>
        public float Distance;

        /// <summary>
        /// X-coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y-coordinate.
        /// </summary>
        public int Y { get; set; }

        private long _turnSeen;

        /// <summary>
        /// Creates a new tile with the specified graphic index and traversability.
        /// </summary>
        /// <param name="graphicIndex">Graphics index for this tile.</param>
        /// <param name="traversable">Whether or not this tile is traversable by actors.</param>
        public Tile(int graphicIndex, bool traversable)
        {
            this.GraphicIndex = graphicIndex;
            this.Traversable = traversable;
            this._turnSeen = -1;
            this.Distance = 1000;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // Do nothing.
        }

        public void SetTurnSeen()
        {
            _turnSeen = Session.Player.CurrentTurn;
        }
    }
}
