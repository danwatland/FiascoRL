using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FiascoRL.World;

namespace FiascoRL.Entities.ArtificialIntelligence
{
    class BasicAI : AI
    {
        public BasicAI(Creature host)
            : base(host)
        {

        }

        public override void DoPrioritizedAction()
        {
            int x = Host.Coords.X;
            int y = Host.Coords.Y;
            Tile t = Host.CurrentLevel.TileMap[x, y];

            if (t.TurnSeen >= Session.Player.CurrentTurn - 3) // In LOS or was just in LOS
            {
                Point closest = GetClosestPoint(x, y);
                if (closest == Session.Player.Coords)
                {
                    Host.MeleeAttack(Session.Player);
                }
                else if (Host.CurrentLevel.TileMap[closest.X, closest.Y].Traversable)
                {
                    // Check if any other actors are occupying the same spot
                    bool occupied = Host.CurrentLevel.ActorList.Where(c => c.GetType() == typeof(Creature) && 
                        c.Coords == closest)
                        .Any();

                    if (!occupied)
                    {
                        Host.Coords = closest;
                    }
                }
            }
        }
    }
}
