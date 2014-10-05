using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FiascoRL.Entities;
using FiascoRL.World;

namespace FiascoRL.Etc
{
    /// <summary>
    /// Class used to retrieve objects in line of sight for an actor.
    /// </summary>
    public class LOS
    {
        private Delta[] deltas = { new Delta(0, -1, -1, -1), new Delta(-1, 0, -1, 1), 
                                   new Delta(-1, 0, -1, -1), new Delta(0, 1, -1, 1), 
                                   new Delta(0, 1, 1, 1), new Delta(1, 0, 1, 1), 
                                   new Delta(1, 0, 1, -1), new Delta(0, -1, 1, -1) };
        private int range;
        private Actor actor;
        private List<Tile> inSight;

        /// <summary>
        /// Create a new LOS tracker for the specified actor.
        /// </summary>
        /// <param name="actor">Actor to track LOS for.</param>
        public LOS(Actor actor)
        {
            this.actor = actor;
            this.range = 6;
        }

        /// <summary>
        /// Returns a list of all visible tiles.
        /// </summary>
        /// <returns>List of all visible tiles.</returns>
        public List<Tile> GetVisible()
        {
            inSight = new List<Tile>();

            foreach (Delta d in deltas)
            {
                RecursiveShadowCast((int)actor.Coords.X, (int)actor.Coords.Y, d);
            }

            return inSight;
        }

        private void RecursiveShadowCast(int x, int y, Delta d)
        {
            // Check if this is within circular max range.
            Tile tile = actor.CurrentLevel.TileMap[x, y];
            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(actor.Coords.X, actor.Coords.Y));
            if (distance <= range)
            {
                if (!tile.Traversable) // If tile can't be seen through
                {
                    if (!inSight.Contains(tile))
                    {
                        tile.Distance = distance;
                        tile.SetTurnSeen();
                        inSight.Add(tile);
                    }

                }
                else // If tile can be seen through
                {
                    if (!inSight.Contains(tile))
                    {
                        tile.Distance = distance;
                        tile.SetTurnSeen();
                        inSight.Add(tile);
                    }

                    RecursiveShadowCast(x + d.dx1, y + d.dy1, d);
                    RecursiveShadowCast(x + d.dx2, y + d.dy2, d);
                }
            }
        }

        private class Delta
        {
            public int dx1, dx2, dy1, dy2;

            public Delta(int dx1, int dy1, int dx2, int dy2)
            {
                this.dx1 = dx1;
                this.dy1 = dy1;
                this.dx2 = dx2;
                this.dy2 = dy2;
            }
        }
    }
}
