using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FiascoRL.Display.UI;
using FiascoRL.Entities;
using FiascoRL.Etc.ExtensionMethods;
using FiascoRL.World;
using FiascoRL.Display.UI.Controls;
using FiascoRL.Combat.Skills;
using FiascoRL.Display;

namespace FiascoRL.Input
{
    /// <summary>
    /// Collection of methods and variables to handle game input.
    /// </summary>
    public static class InputHandler
    {
        private static float _elapsedTime;
        private const float InputInterval = 0.165f;
        private static bool _keyPressed;
        private static bool _targeting;

        /// <summary>
        /// Method to handle all keyboard input.
        /// </summary>
        public static void HandleInput(FiascoGame game, GameTime gameTime, Level level)
        {
            KeyboardState kstate = Keyboard.GetState();
            bool waitPressed = false;
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.GetPressedKeys().Length == 0)
            {
                _keyPressed = false;
            }

            if (_elapsedTime >= InputInterval || _keyPressed == false)
            {
                Point delta = new Point(0, 0);

                if (kstate.IsKeyDown(Keys.NumPad1) || kstate.IsKeyDown(Keys.B)) // Down-left
                {
                    delta = new Point(-1, 1);
                }
                else if (kstate.IsKeyDown(Keys.NumPad2) || kstate.IsKeyDown(Keys.J)) // Down
                {
                    delta = new Point(0, 1);
                }
                else if (kstate.IsKeyDown(Keys.NumPad3) || kstate.IsKeyDown(Keys.N)) // Down-right
                {
                    delta = new Point(1, 1);
                }
                else if (kstate.IsKeyDown(Keys.NumPad4) || kstate.IsKeyDown(Keys.H)) // Left
                {
                    delta = new Point(-1, 0);
                }
                else if (kstate.IsKeyDown(Keys.NumPad5)) // Wait key
                {
                    waitPressed = true;
                }
                else if (kstate.IsKeyDown(Keys.NumPad6) || kstate.IsKeyDown(Keys.L)) // Right
                {
                    delta = new Point(1, 0);
                }
                else if (kstate.IsKeyDown(Keys.NumPad7) || kstate.IsKeyDown(Keys.Y)) // Up-left
                {
                    delta = new Point(-1, -1);
                }
                else if (kstate.IsKeyDown(Keys.NumPad8) || kstate.IsKeyDown(Keys.K)) // Up
                {
                    delta = new Point(0, -1);
                }
                else if (kstate.IsKeyDown(Keys.NumPad9) || kstate.IsKeyDown(Keys.U)) // Up-right
                {
                    delta = new Point(1, -1);
                }
                else if (kstate.IsKeyDown(Keys.G)) // Pick up item
                {
                    PickUpItems(level);
                    RegisterKeyPress();
                }
                else if (kstate.IsKeyDown(Keys.I)) // Open/close inventory
                {
                    Session.UIManager.Controls.GetUIComponent(typeof(InventoryControl)).Enabled =
                        !Session.UIManager.Controls.GetUIComponent(typeof(InventoryControl)).Enabled;
                    RegisterKeyPress();
                }
                else if (kstate.IsKeyDown(Keys.M)) // Open/close minimap
                {
                    Session.UIManager.Controls.GetUIComponent(typeof(MinimapControl)).Enabled =
                        !Session.UIManager.Controls.GetUIComponent(typeof(MinimapControl)).Enabled;
                    RegisterKeyPress();
                }
                else if (kstate.IsKeyDown(Keys.OemPeriod) && (kstate.IsKeyDown(Keys.LeftShift) || kstate.IsKeyDown(Keys.RightShift)))
                {
                    if (Session.Player.CurrentLevel.DecorationMap[Session.Player.Coords.X, Session.Player.Coords.Y].GetType() == typeof(Staircase))
                    {
                        var stairs = (Staircase)Session.Player.CurrentLevel.DecorationMap[Session.Player.Coords.X, Session.Player.Coords.Y];
                        if (stairs.ConnectingStaircase == null)
                        {
                            Level newLevel = new Cave(40, 40)
                            {
                                Depth = Session.Player.CurrentLevel.Depth + 1,
                                LevelTexture = SpriteGraphic.World,
                            };
                            newLevel.GenerateLevel();
                            Point p = newLevel.GetRandomOpenTile();
                            newLevel.DecorationMap[p.X, p.Y] = new Staircase(newLevel.StaircaseUp) { Coords = p, Texture = SpriteGraphic.World, Level = newLevel };
                            stairs.ConnectStaircase((Staircase)newLevel.DecorationMap[p.X, p.Y]);
                        }
                        stairs.Level = Session.Player.CurrentLevel;
                        Session.Player.CurrentLevel.ActorList.Remove(Session.Player);
                        stairs.ConnectingStaircase.Level.ActorList.Add(Session.Player);
                        Session.Player.CurrentLevel = stairs.ConnectingStaircase.Level;
                        Session.Player.Coords = stairs.ConnectingStaircase.Coords;
                    }
                    RegisterKeyPress();
                }
                else if (kstate.IsKeyDown(Keys.D1))
                {
                    Session.Player.Target.TargetType = Etc.Targeting.Target.TargetingType.Diamond;
                    Session.Player.Target.AreaType = Etc.Targeting.Target.TargetingType.SingleSquare;
                    Session.Player.Target.Visible = !Session.Player.Target.Visible;
                    _targeting = !_targeting;
                    RegisterKeyPress();
                }
                else if (kstate.IsKeyDown(Keys.Enter))
                {
                    if (_targeting)
                    {
                        Creature c = Session.Player.CurrentLevel.GetCreatureAt(new Point(
                            Session.Player.Coords.X + Session.Player.Target.TargetedCoords.X,
                            Session.Player.Coords.Y + Session.Player.Target.TargetedCoords.Y));
                        if (c != null)
                        {
                            Spell fireball = Spell.Fireball(Session.Player, c);
                            fireball.Perform(Session.Player, c);
                            Session.Player.Target.Visible = false;
                            _targeting = false;
                            Session.Player.CurrentLevel.ProcessCreatureTurns();
                            Session.Player.CurrentTurn++;
                            RegisterKeyPress();
                        }
                        
                    }
                }

                if (!(delta.X == 0 && delta.Y == 0) || waitPressed)
                {
                    if (_targeting)
                    {
                        MoveTarget(delta);
                    }
                    else
                    {
                        PerformMovement(delta);
                    }
                }
            }
        }

        #region Action methods
        private static void PerformMovement(Point delta)
        {
            int x = Session.Player.Coords.X + delta.X;
            int y = Session.Player.Coords.Y + delta.Y;

            if (Session.Player.CurrentLevel.TileMap[x, y].Traversable)
            {
                Level currentLevel = Session.Player.CurrentLevel;
                Creature creature = currentLevel.GetCreatureAt(new Point(x, y));

                if (currentLevel.TileMap[x, y].Traversable)
                {
                    if (creature == null)
                    {
                        Session.Player.Coords = new Point(x, y);
                    }
                    else if (creature != Session.Player)
                    {
                        Session.Player.MeleeAttack(creature);
                    }
                    Session.Player.LOS.GetVisible();

                    Session.Player.CurrentLevel.ProcessCreatureTurns();

                    Session.Player.CurrentTurn++;
                    RegisterKeyPress();
                }
            }
        }

        private static void MoveTarget(Point delta)
        {
            Session.Player.Target.MoveCoords(delta);
            RegisterKeyPress();
        }

        private static void PickUpItems(Level level)
        {
            Point playerCoords = Session.Player.Coords;
            var items = level.GetItemsAt(playerCoords);

            if (items == null)
            {
                return;
            }
            else if (items != null && items.Count() == 1)
            {
                Session.Player.AddItem(items[0]);
                level.ActorList.Remove(items[0]);
            }
            else
            {
                // TODO: Implement a multi-item pickup.
            }
        }
        #endregion

        #region Internal helper methods
        private static void RegisterKeyPress()
        {
            _elapsedTime = 0;
            _keyPressed = true;
        }
        #endregion
    }
}
