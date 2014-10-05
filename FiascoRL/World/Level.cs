using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Entities;
using FiascoRL.Entities.Util;
using FiascoRL.Etc.WeightedRandom;
using FiascoRL.World;
using FiascoRL.Display;

namespace FiascoRL.World
{

    public abstract class Level
    {
        /// <summary>
        /// Texture types within the World spritesheet.
        /// </summary>
        protected enum LevelTypes
        {
            StructureGray = 0,
            StructureWornGray = 1,
            StructureCrackedBrown = 2,
            PointyRed = 3,
            CryptBrown = 4,
            TempleGreen = 5,
            TempleTeal = 6,
            AquaductBlue = 7,
            StructureGold = 8,
            SewerGray = 9,
            StructureTeal = 10,
            StructureWornBrown = 11,
            StructureMossyBrown = 12,
            CaveBrown = 13,
            ForestGreen = 14,
            TempleUndead = 15
        }

        protected const int TilesetColumns = 55;

        /// <summary>
        /// Texture type to use in this level.
        /// </summary>
        protected int LevelType { get; set; }

        /// <summary>
        /// Width of this level in tiles.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of this level in tiles.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Depth (difficulty) of this level.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Two dimensional array of tiles representing the layout of this floor.
        /// </summary>
        public Tile[,] TileMap { get; set; }

        /// <summary>
        /// Two dimensional array of decoration tiles.
        /// </summary>
        public Tile[,] DecorationMap { get; set; }

        /// <summary>
        /// All actors currently on this level.
        /// </summary>
        public List<Actor> ActorList
        {
            get
            {
                if (_actorList == null)
                {
                    _actorList = new List<Actor>();
                }
                return _actorList;
            }
            set
            {
                _actorList = value;
            }
        } 
        private List<Actor> _actorList;

        /// <summary>
        /// Texture file to be loaded for this level.
        /// </summary>
        public Texture2D LevelTexture { get; set; }

        /// <summary>
        /// Variable containing index of the graphic needed.
        /// </summary>
        public int FloorA = 3,
                   Wall = 9,
                   StaircaseDown = 8,
                   StaircaseUp = 7,
                   E_Wall = 10,
                   EW_Wall = 11,
                   W_Wall = 12,
                   S_Wall = 13,
                   NS_Wall = 14,
                   N_Wall = 15,
                   SE_Wall = 16,
                   SW_Wall = 17,
                   NE_Wall = 18,
                   NW_Wall = 19,
                   NWSE_Wall = 20,
                   WSE_Wall = 21,
                   NWS_Wall = 22,
                   NES_Wall = 23,
                   NWE_Wall = 24;

        /// <summary>
        /// Random number generator.
        /// </summary>
        protected Random Rand;

        /// <summary>
        /// Creates a new level with the specified height and width boundaries.
        /// </summary>
        /// <param name="width">Width of the level, in tiles.</param>
        /// <param name="height">Height of the level, in tiles.</param>
        public Level(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.TileMap = new Tile[width, height];
            this.DecorationMap = new Tile[width, height];
            this.LevelTexture = SpriteGraphic.World;
            this.ActorList = new List<Actor>();
            this.Rand = new Random();
        }

        /// <summary>
        /// Generates this level.
        /// </summary>
        public abstract void GenerateLevel();

        /// <summary>
        /// Returns number of non-traversable tiles surrounding the specified tile.
        /// </summary>
        /// <param name="x">X-coordinate of tile.</param>
        /// <param name="y">Y-coordinate of tile.</param>
        /// <returns></returns>
        protected int NumberOfNeighbors(int x, int y)
        {
            int[] xArr = { x - 1, x, x + 1, x + 1, x + 1, x, x - 1, x - 1 };
            int[] yArr = { y - 1, y - 1, y - 1, y, y + 1, y + 1, y + 1, y };

            int count = 0;
            for (int i = 0; i < xArr.Length; i++)
            {
                if (xArr[i] >= 0 && xArr[i] < this.Width && yArr[i] >= 0 && yArr[i] < this.Height)
                {
                    if (TileMap[xArr[i], yArr[i]].Traversable == false)
                        count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Returns a random traversable tile in this level.
        /// </summary>
        /// <returns>Random traversable tile.</returns>
        public Point GetRandomOpenTile()
        {
            List<Point> pointList = new List<Point>();
            var creatureCoords = ActorList.Where(x => x.GetType() == typeof(Creature)).Select(x => x.Coords);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (TileMap[x, y].Traversable)
                    {
                        Point p = new Point(x, y);

                        if (!creatureCoords.Contains(p))
                        {
                            pointList.Add(new Point(x, y));
                        }
                    }
                }
            }

            return pointList[Rand.Next(pointList.Count)];
        }

        /// <summary>
        /// Returns the creature at the specified location in this level or null if no creatures are present.
        /// </summary>
        /// <param name="p">Location to search.</param>
        /// <returns>Creature at the specified location or null if none are present.</returns>
        public Creature GetCreatureAt(Point p)
        {
            var result = ActorList.Where(x => x.GetType() == typeof(Creature) && x.Coords == p);

            if (result.Count() > 0)
            {
                return result.Cast<Creature>().First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the item(s) at the specified location in this level or null if no items are present.
        /// </summary>
        /// <param name="p">Location to search.</param>
        /// <returns>Item(s) at the specified location in this level or null if no items are present.</returns>
        public List<Item> GetItemsAt(Point p)
        {
            var result = ActorList.Where(x => x.GetType() == typeof(Item) && x.Coords == p);

            if (result.Count() > 0)
            {
                return result.Cast<Item>().ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adds a stairway accessible from the given point.
        /// </summary>
        /// <param name="point">Point from which the stairway needs to be accessible from.</param>
        /// <param name="direction">Whether the stairs are going up or down.</param>
        /// <returns></returns>
        public Staircase AddRandomAccessibleStaircase(Point point, Staircase.StairType direction)
        {
            int graphicIndex = (direction == Staircase.StairType.Down) ? StaircaseDown : StaircaseUp;

            _accessibleArea = new List<Point>();
            List<Point> points = GetAccessibleArea(point);

            Point stairCoord = points[Rand.Next(points.Count)];
            DecorationMap[stairCoord.X, stairCoord.Y] = new Staircase(graphicIndex)
            {
                Coords = stairCoord,
                Texture = SpriteGraphic.World, 
            };

            return (Staircase)DecorationMap[stairCoord.X, stairCoord.Y]; 
        }


        /// <summary>
        /// Adds creatures to this level's ActorList.
        /// </summary>
        protected void PopulateWithCreatures()
        {
            int baseCreatures = 8;
            int additionalCreatures = Rand.Next(8);
            for (int i = 0; i < baseCreatures + additionalCreatures; i++)
            {
                WeightedRandom wr = RandomGenerator.Generate(ObjectLists.MonsterList(), 1); // TODO: Make variable.
                Creature c = Creature.GenerateCreature((CreatureType)wr.Type, this);
                c.Coords = GetRandomOpenTile();
                ActorList.Add(c);

            }
        }

        /// <summary>
        /// Adds items to this level's ActorList.
        /// </summary>
        protected void PopulateWithItems()
        {
            int baseItems = 50;
            int additionalItems = Rand.Next(4);
            for (int i = 0; i < baseItems + additionalItems; i++)
            {
                WeightedRandom wr = RandomGenerator.Generate(ObjectLists.ItemList(), 1);
                Item item = Item.GenerateItem((ItemType)wr.Type);
                item.CurrentLevel = this;
                item.Coords = GetRandomOpenTile();
                ActorList.Add(item);
            }
        }

        /// <summary>
        /// Encloses the level in walls.
        /// </summary>
        protected void EncloseLevel()
        {
            for (int x = 0; x < this.Width; x++)
            {
                TileMap[x, 0] = new Tile(Wall + LevelType * TilesetColumns, false);
                TileMap[x, this.Height - 1] = new Tile(Wall + LevelType * TilesetColumns, false);
            }

            for (int y = 0; y < this.Height; y++)
            {
                TileMap[0, y] = new Tile(Wall + LevelType * TilesetColumns, false); ;
                TileMap[this.Width - 1, y] = new Tile(Wall + LevelType * TilesetColumns, false); ;
            }
        }
        
        /// <summary>
        /// Process turns for all creatures in level.
        /// </summary>
        public void ProcessCreatureTurns()
        {
            ActorList.Where(c => typeof(Creature).IsAssignableFrom(c.GetType()))
                .Cast<Creature>()
                .ToList()
                .ForEach(c => c.ProcessTurn());

            // Remove creatures with no health.
            ActorList.Where(c => c.GetType() == typeof(Creature) && ((Creature)c).HP.Current <= 0)
                .Cast<Creature>()
                .Select(i => new Tuple<Creature, Item>(i, i.GetItems().FirstOrDefault()))
                .ToList()
                .ForEach(c => {
                    c.Item2.Coords = c.Item1.Coords;
                    ActorList.Add(c.Item2);
                });

            ActorList = ActorList.Where(c => (c.GetType() == typeof(Creature) && ((Creature)c).HP.Current > 0)
                || c.GetType() != typeof(Creature))
                .ToList();
        }

        /// <summary>
        /// Edits walls to look better.
        /// </summary>
        protected void EditWalls()
        {
            int[] xArr = { 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] yArr = { -1, -1, 0, 1, 1, 1, 0, -1 };

            // Change to account for edge walls.
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    List<Tile> surroundingTiles = new List<Tile>();
                    for (int i = 0; i < xArr.Length; i++)
                    {
                        if (x + xArr[i] >= 0 && x + xArr[i] < Width && y + yArr[i] >= 0 && y + yArr[i] < Height)
                        {
                            surroundingTiles.Add(TileMap[x + xArr[i], y + yArr[i]]);
                        }
                        else
                        {
                            surroundingTiles.Add(new Tile(0, true));
                        }
                            
                    }
                    
                    int[] walls = surroundingTiles.Select((tile, index) => new { Tile = tile, Index = index })
                                                .Where(t => !t.Tile.Traversable)
                                                .Select(i => i.Index).ToArray();
                    if (!TileMap[x, y].Traversable && walls.Length > 0 && walls.Length < 8)
                    {
                        if (!(new int[] { 0, 2, 4, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NWSE_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0, 2, 4 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NES_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0, 2, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NWE_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0, 4, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NWS_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 2, 4, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = WSE_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0, 2 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NE_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0, 4 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NS_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = NW_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 2, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = EW_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 2, 4 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = SE_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 4, 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = SW_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 0 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = N_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 2 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = E_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 4 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = S_Wall + LevelType * TilesetColumns;
                        }
                        else if (!(new int[] { 6 }).Except(walls).Any())
                        {
                            TileMap[x, y].GraphicIndex = W_Wall + LevelType * TilesetColumns;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Adds shadows to open tiles with a wall above them.
        /// </summary>
        protected void AddShadowsToWalls()
        {
            for (int x = 0; x < TileMap.GetLength(0); x++)
            {
                for (int y = 1; y < TileMap.GetLength(1); y++)
                {
                    if (TileMap[x, y].Traversable && !TileMap[x, y - 1].Traversable)
                    {
                        DecorationMap[x, y] = new Tile(2010, true);
                    }
                }
            }
        }

        #region Helper methods
        private List<Point> _accessibleArea;
        private readonly Point[] _surr = new Point[] { new Point(1, 1), new Point(1, 0), new Point(1, -1),
            new Point(0, -1), new Point(-1, -1), new Point(-1, 0), new Point(-1, 1), new Point(0, 1) };

        private List<Point> GetAccessibleArea(Point p)
        {
            foreach (var point in _surr)
            {
                Point pointToCheck = new Point(p.X + point.X, p.Y + point.Y);
                if (!_accessibleArea.Contains(pointToCheck) && 
                    TileMap[pointToCheck.X, pointToCheck.Y].Traversable)
                {
                    _accessibleArea.Add(pointToCheck);
                    GetAccessibleArea(new Point(p.X + point.X, p.Y + point.Y));
                }
            }

            return _accessibleArea;
        }
        #endregion
    }
}
