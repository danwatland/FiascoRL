using FiascoRL.Etc.ExtensionMethods;
using FiascoRL.Etc.WeightedRandom;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.World
{
    public class Dungeon : Level
    {
        public Dungeon(int width, int height)
            : base(width, height)
        {
            this.LevelType = (int)LevelTypes.StructureGray;
            this._rand = RandomGenerator.Rand;
            this.Rooms = new List<Room>();
        }

        public double Density { get; set; }

        public override void GenerateLevel()
        {
            TileMap.PerformAction(f => f = new Tile(Wall, true));

            // Create rooms...
            Density = 0.50;
            int numberOfRooms = GetMaxNumberOfRooms();
            while (Rooms.Count < numberOfRooms)
            {
                PlaceRandomRoom();
            }

            foreach (var room in Rooms)
            {
                CreateRoomTiles(room);
            }

            EncloseLevel();
        }

        private Random _rand;
        private List<Room> Rooms;

        #region Room methods
        private bool DoRoomsIntersect(Room r1, Room r2)
        {
            return r1.Coords.Left <= r2.Coords.Right && r1.Coords.Right >= r2.Coords.Left &&
                   r1.Coords.Top <= r2.Coords.Bottom && r1.Coords.Bottom >= r2.Coords.Top;
        }

        private Room GetRandomPassageway()
        {
            int height, width;
            int length = _rand.Next(Room.MAX_LENGTH - Room.MIN_LENGTH) + Room.MIN_LENGTH;
            if (_rand.NextDouble() > 0.5)
            {
                height = length;
                width = 3;
            }
            else
            {
                height = 3;
                width = length;
            }

            int x = _rand.Next(Width - width);
            int y = _rand.Next(Height - height);
            Rectangle rect = new Rectangle(x, y, width, height);
            return new Room(rect);
        }

        private Room GetRandomRoom()
        {
            int height = _rand.Next(Room.MAX_LENGTH - Room.MIN_LENGTH) + Room.MIN_LENGTH;
            int width = _rand.Next(Room.MAX_LENGTH - Room.MIN_LENGTH) + Room.MIN_LENGTH;

            int x = _rand.Next(Width - width);
            int y = _rand.Next(Height - height);
            Rectangle rect = new Rectangle(x, y, width, height);
            return new Room(rect);
        }

        private bool PlaceRandomRoom()
        {
            var proposedRoom = GetRandomRoom();
            foreach (var existingRoom in Rooms)
            {
                if (DoRoomsIntersect(existingRoom, proposedRoom))
                    return false;
            }
            Rooms.Add(proposedRoom);
            return true;
        }

        private void CreateRoomTiles(Room room)
        {
            for (int x = room.Coords.Left; x <= room.Coords.Right; x++)
            {
                for (int y = room.Coords.Top; y <= room.Coords.Bottom; y++)
                {
                    if (TileMap[x, y] == null)
                    {
                        TileMap[x, y] = new Tile(Floor, true);
                    }
                    else
                    {
                        TileMap[x, y].GraphicIndex = Floor;
                        TileMap[x, y].Traversable = true;
                    }
                }
            }

            var edges = room.GetEdges();
            foreach (var point in edges)
            {
                TileMap[point.X, point.Y].GraphicIndex = Wall;
            }
        }

        struct Room
        {
            public Rectangle Coords { get { return _coords; } }
            private Rectangle _coords;
            public const int MAX_LENGTH = 11;
            public const int MIN_LENGTH = 6;

            public Room(Rectangle rect)
            {
                _coords = rect;
            }

            public List<Point> GetEdges()
            {
                List<Point> edges = new List<Point>();
                for (int x = Coords.X; x <= Coords.X + Coords.Width; x++)
                {
                    edges.Add(new Point(x, Coords.Y));
                    edges.Add(new Point(x, Coords.Y + Coords.Height));
                }

                for (int y = Coords.Y; y < Coords.Y + Coords.Height; y++)
                {
                    edges.Add(new Point(Coords.X, y));
                    edges.Add(new Point(Coords.X + Coords.Width, y));
                }

                return edges;
            }
        }
        #endregion

        #region Helper methods
        private int GetMaxNumberOfRooms()
        {
            int levelTiles = Height * Width;
            double roomTiles = Math.Pow(((double)Room.MAX_LENGTH + Room.MIN_LENGTH) / 2, 2);
            int numRooms = (int)(levelTiles * Density / roomTiles);
            return numRooms;
        }
        #endregion
    }
}
