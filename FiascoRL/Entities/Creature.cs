using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Display;
using FiascoRL.Display.Animation;
using FiascoRL.Entities.ArtificialIntelligence;
using FiascoRL.Entities.Util;
using FiascoRL.Etc.WeightedRandom;
using FiascoRL.World;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FiascoRL.Entities
{
    public class Creature : Actor
    {
        public Creature(Texture2D texture, int graphicIndex)
            : base(texture, graphicIndex)
        {
            this.baseRegenSpeed = 55;
            this.Items = new List<Item>();
        }

        /// <summary>
        /// Name of this creature.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Variable representing hit points of this creature.
        /// </summary>
        public Stat HP { get; set; }

        /// <summary>
        /// Variable representing magic points of this creature.
        /// </summary>
        public Stat SP { get; set; }

        /// <summary>
        /// Variable representing strength of this creature.
        /// </summary>
        public Stat Strength { get; set; }

        private AI ai;
        protected double baseRegenSpeed;

        #region Items

        /// <summary>
        /// Item(s) this creature is carrying.
        /// </summary>
        protected List<Item> Items { get; set; }

        /// <summary>
        /// Equipped item(s) this creature is carrying.
        /// </summary>
        public ObservableCollection<Equippable> EquippedItems
        {
            get
            {
                if (_equippedItems == null)
                {
                    _equippedItems = new ObservableCollection<Equippable>();
                    _equippedItems.CollectionChanged += OnEquippableItemAdded;
                }
                return _equippedItems;
            }
            set { _equippedItems = value; }
        } 
        private ObservableCollection<Equippable> _equippedItems;

        private void OnEquippableItemAdded(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
            {
                ModifyEquippables(args.NewItems.Cast<Equippable>().ToList());
            }
            if (args.OldItems != null)
            {
                ModifyEquippables(args.NewItems.Cast<Equippable>().ToList(), true);
            }
        }

        private void ModifyEquippables(List<Equippable> items, bool subtract = false)
        {
            int sign = subtract ? -1 : 1;
            foreach (var item in items)
            {
                var itemStats = typeof(Equippable)
                    .GetProperties()
                    .Where(x => x.PropertyType == typeof(int))
                    .Select(x => new { Stat = (int)x.GetValue(item, null), Name = x.Name });
                this.GetType().GetProperties()
                    .Where(x => x.PropertyType == typeof(Stat))
                    .ToList()
                    .ForEach(x =>
                    {
                        if (x.GetValue(this, null) != null)
                        {
                            ((Stat)x.GetValue(this, null)).Additional +=
                                (itemStats.Where(t => t.Name == x.Name).Any()) ?
                                sign * itemStats.Where(t => t.Name == x.Name).First().Stat :
                                0;
                        }
                    });
            }
        }

        #endregion

        /// <summary>
        /// This creature's shadow index. Only used for flying creatures.
        /// </summary>
        public int ShadowIndex { get; set; }

        /// <summary>
        /// Generates a new creature of the given CreatureType.
        /// </summary>
        /// <param name="type">CreatureType of creature to generate.</param>
        /// <returns>New creature of the given CreatureType.</returns>
        public static Creature GenerateCreature(CreatureType type, Level level)
        {
            Creature c = new Creature(SpriteGraphic.Creatures, type.GraphicIndex)
            {
                Name = type.Name,
                HP = new Stat(type.HP, type.HP),
                SP = new Stat(type.MP, type.MP),
                ShadowIndex = type.ShadowIndex,
                CurrentLevel = level,
            };
            c.ai = (AI)Activator.CreateInstance(type.AIClass, c);
            WeightedRandom wr = RandomGenerator.Generate(ObjectLists.ItemList(), level.Depth);
            Item item = Item.GenerateItem((ItemType)wr.Type);
            c.Items.Add(item);
            return c;
        }

        /// <summary>
        /// Returns this creature's damage animation.
        /// </summary>
        /// <returns>This creature's damage animation.</returns>
        public Animation DamageAnimation()
        {
            var anim = new Animation(this, 8)
            {
                FrameLength = 0.04f,
                Texture = SpriteGraphic.Creatures,
                Offset = new Point(0, -2),
            };
            anim.SetColors(Color.Red);
            anim.AddLinearFade();

//            for (int i = 0; i < 8; i++)
//            {
//                anim.Colors.Add(new Microsoft.Xna.Framework.Color(1.0f, 0.0f, 0.0f, (float)(1.0 - 0.125 * i)));
//            }

            return anim;
        }

        /// <summary>
        /// AI of this creature.
        /// </summary>
        public AI CreatureAI { set { ai = value; } }

        /// <summary>
        /// Causes creature to perform the highest priority action as determined by their AI.
        /// </summary>
        private void DoPrioritizedAction()
        {
            ai.DoPrioritizedAction();
        }

        /// <summary>
        /// Performs all calculations needed to process a turn for this creature and performs the prioritized action per the creature's AI.
        /// </summary>
        public void ProcessTurn()
        {
            if (HP.Current > 0)
            {
                // Regen health.
                int regenRate = (int)Math.Max(baseRegenSpeed, 1);
                if (Session.Player.CurrentTurn % regenRate == 0)
                {
                    int regenAmount = (int)Math.Max(1 / regenRate, 1);
                    HP.Current = Math.Min(HP.Max, HP.Current + regenAmount);
                }

                // Player does not have an AI.
                if (!(this is Player))
                    DoPrioritizedAction();
            }
        }

        /// <summary>
        /// Performs a melee attack against the given recipient.
        /// </summary>
        /// <param name="recipient"></param>
        public void MeleeAttack(Creature recipient)
        {
            // TODO: Logic for determining how much damage is done
            recipient.HP.Current--;
            Session.Animations.Add(recipient.DamageAnimation());
            Session.TextAnimations.Add(StaticAnimations.DamageTextAnimation(recipient, 1));
            Session.MessageLog.Enqueue(Name + " hits " + recipient.Name + " for 1 damage.");
        }

        /// <summary>
        /// Adds an item to the creature's inventory.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void AddItem(Item item)
        {
            if (Items.Contains(item))
            {
                Items.Where(x => x.Equals(item)).First().Quantity += item.Quantity;
            }
        }

        /// <summary>
        /// Returns the list of items currently in this creature's inventory.
        /// </summary>
        /// <returns>List of items currently in this creature's inventory.</returns>
        public List<Item> GetItems()
        {
            return Items;
        }
    }
}
