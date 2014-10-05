using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiascoRL.Entities.ArtificialIntelligence;

namespace FiascoRL.Entities.Util
{

        /// <summary>
        /// Class containing pre-defined creature types and variables.
        /// </summary>
        public class CreatureType: ObjectType
        {
            public static readonly CreatureType GOBLIN = new CreatureType("Goblin", 6, 0, 5, 118, 401, typeof(BasicAI));
            public static readonly CreatureType BROWN_RAT = new CreatureType("Brown Rat", 3, 0, 2, 224, 401, typeof(BasicAI));
            public static readonly CreatureType GREEN_SNAKE = new CreatureType("Green Snake", 4, 0, 3, 225, 401, typeof(BasicAI));

            private readonly string name;
            private readonly int hp, mp, graphicIndex, shadow;
            private readonly long exp;
            private readonly Type ai;

            CreatureType(string name, int hp, int mp, long exp, int graphicIndex, int shadow, Type ai)
            {
                this.name = name;
                this.hp = hp;
                this.mp = mp;
                this.exp = exp;
                this.graphicIndex = graphicIndex;
                this.shadow = shadow;
                this.ai = ai;
            }

            public string Name { get { return name; } }

            public int HP { get { return hp; } }

            public int MP { get { return mp; } }

            public long EXP { get { return exp; } }

            public int GraphicIndex { get { return graphicIndex; } }

            public int ShadowIndex { get { return shadow; } }

            public Type AIClass { get { return ai; } }
        }
}
