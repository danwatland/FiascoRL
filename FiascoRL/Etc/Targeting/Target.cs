using FiascoRL.Display.Animation;
using FiascoRL.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Etc.Targeting
{
    public class Target
    {
        public Target(Player player)
        {
            this._player = player;
            this._targetedCoords = new Point(0, 0);
            this.AreaType = TargetingType.SingleSquare;
        }

        /// <summary>
        /// Type of target to be displayed.
        /// </summary>
        public TargetingType TargetType { get; set; }

        /// <summary>
        /// Type of target area to be displayed.
        /// </summary>
        public TargetingType AreaType { get; set; }

        /// <summary>
        /// Currently targeted coordinates.
        /// </summary>
        public Point TargetedCoords
        {
            get
            {
                Point[] currentArea = GetOffsets(TargetType);
                if (!currentArea.Contains(_targetedCoords))
                {
                    _targetedCoords = currentArea.First();
                }
                return _targetedCoords;
            }
            set
            {
                _targetedCoords = value;
            }
        }

        /// <summary>
        /// Whether or not the targeting graphics are drawn to the screen.
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
                if (_visible)
                {
                    var points = GetOffsets(TargetType).Select(x => new Point { X = x.X + Session.Player.Coords.X, Y = x.Y + Session.Player.Coords.Y });
                    var result = points.Intersect(
                        Session.Player.CurrentLevel.ActorList.Where(x => x.GetType() == typeof(Creature))
                        .Select(x => x.Coords))
                        .FirstOrDefault();
                    if (result != Point.Zero)
                    {
                        TargetedCoords = new Point(result.X - Session.Player.Coords.X, result.Y - Session.Player.Coords.Y);
                    }
                }
            }
        }

        /// <summary>
        /// Tries to move the targeted coords in the specified direction. If the result is
        /// within the target area, the TargetedCoords is moved to that point. Otherwise,
        /// nothing happens.
        /// </summary>
        /// <param name="delta">Amount to move the targeted coordinates by.</param>
        public void MoveCoords(Point delta)
        {
            Point newPoint = new Point(TargetedCoords.X + delta.X, TargetedCoords.Y + delta.Y);
            Point[] currentArea = GetOffsets(TargetType);
            if (!currentArea.Contains(newPoint) && newPoint == Point.Zero)
            {
                newPoint = new Point(newPoint.X + delta.X, newPoint.Y + delta.Y);
            }

            if (currentArea.Contains(newPoint))
            {
                TargetedCoords = newPoint;
            }
        }
        
        public void Update(GameTime gameTime)
        {
            AreaAnimation.Update(gameTime);
            TargetAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            foreach (Point point in GetOffsets(TargetType))
            {
                spriteBatch.Draw(BlankTexture,
                    new Rectangle((_player.Coords.X + point.X) * 24,
                        (_player.Coords.Y + point.Y) * 24, 24, 24),
                    AreaAnimation.CurrentColor);
            }

            foreach (Point point in GetOffsets(AreaType))
            {
            spriteBatch.Draw(BlankTexture,
                new Rectangle((_player.Coords.X + point.X + TargetedCoords.X) * 24,
                    (_player.Coords.Y + point.Y + TargetedCoords.Y) * 24, 24, 24),
                    TargetAnimation.CurrentColor);

            }
        }

        public enum TargetingType
        {
            Melee,
            Cross,
            OneByOneSquare,
            Diamond,
            SingleSquare
        }

        private Point[] GetOffsets(TargetingType type)
        {
            switch (type)
            {
                case TargetingType.Melee:
                    return new Point[] { new Point(-1, -1), new Point(-1, 0), new Point(-1, 1), new Point(0, 1), 
                        new Point(1, 1), new Point(1, 0), new Point(1, -1), new Point(0, -1) };
                case TargetingType.Cross:
                    return new Point[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(-1, 0), new Point(0, -1) };
                case TargetingType.OneByOneSquare:
                    return new Point[] { new Point(0, 0), new Point(-1, 0), new Point(-1, 1), new Point(0, 1), 
                        new Point(1, 1), new Point(1, 0), new Point(1, -1), new Point(0, -1), new Point(-1, -1) };
                case TargetingType.Diamond:
                    return new Point[] { new Point(0, 0), new Point(-1, 0), new Point(-1, 1), new Point(0, 1), 
                        new Point(1, 1), new Point(1, 0), new Point(1, -1), new Point(0, -1), new Point(-1, -1),
                        new Point(2, 0), new Point(0, 2), new Point(-2, 0), new Point(0, -2) };
                case TargetingType.SingleSquare:
                    return new Point[] { new Point(0, 0) };
                default:
                    return null;
            }
        }

        internal Animation TargetAnimation
        {
            get
            {
                if (_targetAnimation == null)
                {
                    _targetAnimation = new Animation(10)
                    {
                        FrameLength = 0.025f,
                        Loop = true,
                        Texture = BlankTexture,
                    };

                    for (int i = 0; i < 10; i++)
                    {
                        _targetAnimation.Frames[i].GraphicIndex = 0;
                        _targetAnimation.Frames[i].Color = new Color(152 + i * 10, 155 + i * 10, 30 + i, 127);
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        _targetAnimation.Frames[i].GraphicIndex = 0;
                        _targetAnimation.Frames[i].Color = new Color(252 - i * 10, 255 - i * 10, 40 - i, 127);
                    }

                }
                return _targetAnimation;
            }
        }
        private Animation _targetAnimation;

        internal Animation AreaAnimation
        {
            get
            {
                if (_areaAnimation == null)
                {
                    _areaAnimation = new Animation(10)
                    {
                        FrameLength = 0.05f,
                        Loop = true,
                        Texture = BlankTexture,
                    };

                    for (int i = 0; i < 10; i++)
                    {
                        _areaAnimation.Frames[i].GraphicIndex = 0;
                        _areaAnimation.Frames[i].Color = new Color(80 + i * 6, 80 + i * 6, 80 + i * 6, 127);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        _areaAnimation.Frames[i].GraphicIndex = 0;
                        _areaAnimation.Frames[i].Color = new Color(140 - i * 6, 140 - i * 6, 140 - i * 6, 127);
                    }
                }
                return _areaAnimation;
            }
        }
        private Animation _areaAnimation { get; set; }

        internal Texture2D BlankTexture
        {
            get
            {
                if (_texture == null)
                {
                    _texture = new Texture2D(Session.Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                    Color[] color = new Color[1] { Color.White };
                    _texture.SetData(color);
                }

                return _texture;
            }
        }
        
        private Texture2D _texture;
        private Player _player;
        private Point _targetedCoords;
        private bool _visible;
    }
}
