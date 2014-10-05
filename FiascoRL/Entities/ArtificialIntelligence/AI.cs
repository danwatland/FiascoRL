using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FiascoRL.World;

namespace FiascoRL.Entities.ArtificialIntelligence
{
    public abstract class AI
    {
        /// <summary>
        /// Creature hosting this AI.
        /// </summary>
        public Creature Host { get { return host; } }

        /// <summary>
        /// Performs this creature's prioritized action.
        /// </summary>
        public abstract void DoPrioritizedAction();

        private Creature host;

        public AI(Creature host)
        {
            this.host = host;
        }

        /// <summary>
        /// From the given point, returns the tile closest to the player, if one is available.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <returns>Point representing tile closest to player.</returns>
        protected Point GetClosestPoint(int x, int y)
        {
            int[] dx = { 1, 0, -1, -1, -1, 0, 1, 1 };
            int[] dy = { 1, 1, 1, 0, -1, -1, -1, 0 };
            int playerX = Session.Player.Coords.X;
            int playerY = Session.Player.Coords.Y;

            float distance = Host.CurrentLevel.TileMap[x, y].Distance;
            long turnSeen = Host.CurrentLevel.TileMap[x, y].TurnSeen;
            Point result = new Point(0, 0);

            for (int i = 0; i < dx.Length; i++)
            {
                Tile t = Host.CurrentLevel.TileMap[x + dx[i], y + dy[i]];
                if ((t.Distance < distance || (t.TurnSeen != -1 && t.TurnSeen > turnSeen)) && t.Traversable)
                {
                    var creature = host.CurrentLevel.GetCreatureAt(new Point(x + dx[i], y + dy[i]));
                    if (creature == null || creature == Session.Player)
                    {
                        distance = t.Distance;
                        turnSeen = t.TurnSeen;
                        result = new Point(x + dx[i], y + dy[i]);
                    }
                }
            }

            return result;
        }

    }
}
