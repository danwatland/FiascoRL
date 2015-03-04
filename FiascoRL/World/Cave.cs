using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiascoRL.Etc.ExtensionMethods;
using FiascoRL.World;
using FiascoRL.Display;
using Microsoft.Xna.Framework;

namespace FiascoRL.World
{
    public class
        Cave : Level
    {
        public Cave(int width, int height)
            : base(width, height)
        {
            this.LevelType = (int)LevelTypes.TempleUndead;
        }

        public override void GenerateLevel()
        {
            // For best results, use a value close to 0.5.
            float tolerance = 0.49f;

            TileMap.PerformAction(t =>
            {
                if (Rand.NextDouble() > tolerance) // Floor
                {
                    return new Tile(Floor + TilesetColumns * LevelType, true);
                }
                else // Wall
                {
                    return new Tile(Wall + TilesetColumns * LevelType, false);
                }
            });

            // Utilize celluar automata rule B678/S345678 to generate a natural cave looking layout.
            int count = 1;
            while (count != 0)
            {
                count = 0;

                for (int x = 0; x < this.Width; x++)
                {
                    for (int y = 0; y < this.Height; y++)
                    {
                        int n = NumberOfNeighbors(x, y);

                        if (n >= 6 && TileMap[x, y].GraphicIndex == Floor + TilesetColumns * LevelType)
                        {
                            TileMap[x, y] = new Tile(Wall + TilesetColumns * LevelType, false);
                            count++;
                        }
                        else if (n < 3 && TileMap[x, y].GraphicIndex == Wall + TilesetColumns * LevelType)
                        {
                            TileMap[x, y] = new Tile(Floor + TilesetColumns * LevelType, true);
                            count++;
                        }
                    }
                }
            }

            EncloseLevel();
            EditWalls();
            PopulateWithCreatures();
            PopulateWithItems();
            AddShadowsToWalls();
            AddDecorations();
            EditFloors();
            AddRandomAccessibleStaircase(GetRandomOpenTile(), Staircase.StairType.Down);
        }

        private void EditFloors()
        {
            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    if (TileMap[x, y].GraphicIndex == Floor + TilesetColumns * this.LevelType)
                    {
                        double r = Rand.NextDouble();
                        if (r < 0.03)
                        {
                            TileMap[x, y].GraphicIndex = Floor + TilesetColumns * this.LevelType + 1;
                        }
                        else if (r >= 0.03 && r < 0.05)
                        {
                            TileMap[x, y].GraphicIndex = Floor + TilesetColumns * this.LevelType + 2;
                        }
                        else if (r >= 0.05 && r < 0.1)
                        {
                            TileMap[x, y].GraphicIndex = Floor + TilesetColumns * this.LevelType + 3;
                        }
                    }
                }
            }
        }

        private void AddDecorations()
        {
            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    double r = Rand.NextDouble();
                    if (TileMap[x, y].GraphicIndex == Floor + TilesetColumns * LevelType && DecorationMap[x, y] == null && r < 0.05)
                    {
                        DecorationMap[x, y] = new Tile(31 + Rand.Next(7), true);
                    }
                }
            }
        }
    }
}
