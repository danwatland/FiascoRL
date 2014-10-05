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
            this._rand = new Random();
            this.Rooms = new List<Room>();
        }

        public override void GenerateLevel()
        {
            throw new NotImplementedException();
        }

        private Random _rand;
        private List<Room> Rooms;

        #region Room methods
        private List<Point> GetAvailableEdges()
        {
            List<Point> edges = new List<Point>();
            Rooms.ForEach(x =>
            {
                edges.AddRange(x.GetEdges());
            });

            return edges.Distinct().ToList();
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

        struct Room
        {
            public Rectangle Coords { get { return _coords; } }
            private Rectangle _coords;
            public const int MAX_LENGTH = 11;
            public const int MIN_LENGTH = 5;

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
    }
}
