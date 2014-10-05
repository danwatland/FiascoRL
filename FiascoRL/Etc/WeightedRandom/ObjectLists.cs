using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FiascoRL.Entities.Util;

namespace FiascoRL.Etc.WeightedRandom
{
    /// <summary>
    /// Class containing static references to lists that can be fed to the weighted random generator.
    /// </summary>
    class ObjectLists
    {
        /// <summary>
        /// Returns an array of all weighted random monster choices.
        /// </summary>
        /// <returns>Array of all weighted random monster choices.</returns>
        public static WeightedRandom[] MonsterList()
        {
            return RandomObject.GetObjectList(RandomObject.RandomType.Creature);
        }

        /// <summary>
        /// Returns an array of all weighted random item choices.
        /// </summary>
        /// <returns>Array of all weighted random item choices.</returns>
        public static WeightedRandom[] ItemList()
        {
            return RandomObject.GetObjectList(RandomObject.RandomType.Item);
        }

        class RandomObject : ObjectType, WeightedRandom
        {

            public static RandomObject[] GetObjectList(RandomType randomType)
            {
                Type type = typeof(RandomObject);
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                var objects = fields.Select(x => x.GetValue(null)).Cast<RandomObject>().Where(x => x.RType == randomType).ToArray();

                return objects;
            }

            public RandomObject(int minLevel, int maxLevel, int weight, ObjectType type, RandomType randomType)
            {
                this.MinLevel = minLevel;
                this.MaxLevel = maxLevel;
                this.Weight = weight;
                this.Type = type;
                this._name = type.Name;
                this._graphicIndex = type.GraphicIndex;
                this.RType = randomType;
            }

            private string _name;
            private int _graphicIndex;
     
            public enum RandomType
            {
                Creature,
                Item
            }

            #region Properties
            public int MinLevel { get; set; }
            public int MaxLevel { get; set; }
            public int Weight { get; set; }
            public object Type { get; set; }
            public RandomType RType { get; set; }

            public string Name
            {
                get { return _name; }
            }

            public int GraphicIndex
            {
                get { return _graphicIndex; }
            }
            #endregion

            #region Monsters
            public static RandomObject Goblin = new RandomObject(1, 8, 5, CreatureType.GOBLIN, RandomType.Creature);
            public static RandomObject GreenSnake = new RandomObject(1, 5, 10, CreatureType.GREEN_SNAKE, RandomType.Creature);
            public static RandomObject BrownRat = new RandomObject(1, 4, 12, CreatureType.BROWN_RAT, RandomType.Creature);
            #endregion

            #region Items
            public static RandomObject Gold = new RandomObject(1, 10000, 10, ItemType.GOLD, RandomType.Item);
            public static RandomObject Potion = new RandomObject(1, 10000, 3, ItemType.POTION, RandomType.Item);
            #endregion
        }
    }
}
