using FiascoRL.Display.UI.Controls.Coordinates;
using FiascoRL.Display.UI.Controls.Interfaces;
using FiascoRL.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls
{
    public class MinimapControl : Control, IButtonHandler
    {
        #region Properties and constants
        private Color[,] _minimapColor;
        private readonly Color _mapBackground = new Color(89, 86, 82, 255);

        /// <summary>
        /// Level of zoom for the minimap.
        /// </summary>
        protected int ZoomLevel { get; set; }
        #endregion

        public MinimapControl()
        {
            this.CenterBitmap = new Rectangle(719, 290, 128, 128);
            this.ZoomLevel = 2;

            float relHeight = (float)(128.0 / GraphicsDeviceManager.PreferredBackBufferHeight);
            float relWidth = (float)(128.0 / GraphicsDeviceManager.PreferredBackBufferWidth);
            this.Coords = new RelativeRectangle(new RelativeVector(1.0f, -140.0f), new RelativeVector(0.0f, 12.0f), relWidth, relHeight);

            // Set up colors of minimap in an array for later reference.
            Color[] textureData = new Color[UITexture.Width * UITexture.Height];
            _minimapColor = new Color[128, 128];
            UITexture.GetData<Color>(textureData);
            for (int x = 0; x < 128; x++)
            {
                for (int y = 0; y < 128; y++)
                {
                    _minimapColor[x, y] = textureData[((y + 290) * UITexture.Width) + x + 719];
                }
            }

            AddButtons();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Rectangle rect = GetActualCoords();
            Point player = Session.Player.Coords;

            if (Session.Player.CurrentLevel != null)
            {
                Tile[,] tileMap = Session.Player.CurrentLevel.TileMap;
                var tiles = ConvertTileData(tileMap).Where(x => x.Tile.TurnSeen >= 0);
                tiles.Where(x => x.Tile.Traversable).ToList().ForEach(t =>
                {
                    int x = rect.X + rect.Width / 2 + (t.X - player.X) * ZoomLevel;
                    int y = rect.Y + rect.Height / 2 + (t.Y - player.Y) * ZoomLevel;
                    if (_minimapColor[(int)MathHelper.Clamp((float)x - rect.X, 0, 127), (int)MathHelper.Clamp((float)y - rect.Y, 0, 127)] == _mapBackground)
                    {
                        spriteBatch.Draw(UITexture, new Rectangle(
                            x, y, ZoomLevel, ZoomLevel), new Rectangle(310, 153, 1, 1), Color.Black);
                    }
                });
                tiles.Where(x => !x.Tile.Traversable).ToList().ForEach(t =>
                {
                    int x = rect.X + rect.Width / 2 + (t.X - player.X) * ZoomLevel;
                    int y = rect.Y + rect.Height / 2 + (t.Y - player.Y) * ZoomLevel;
                    if (_minimapColor[(int)MathHelper.Clamp((float)x - rect.X, 0, 127), (int)MathHelper.Clamp((float)y - rect.Y, 0, 127)] == _mapBackground)
                    {
                        spriteBatch.Draw(UITexture, new Rectangle(
                            x, y, ZoomLevel, ZoomLevel), new Rectangle(310, 153, 1, 1), Color.White);
                    }
                });
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MouseState ms = Mouse.GetState();
            Rectangle actualCoords = GetActualCoords();
            Point currentMousePos = new Point(ms.X, ms.Y);

            if (ms.LeftButton == ButtonState.Pressed && Dragging && !ButtonPressed)
            {
                this.Move(ms.X - LastCoordinates.X, ms.Y - LastCoordinates.Y);
            }
            else if (ms.LeftButton == ButtonState.Pressed && !Dragging && actualCoords.Contains(currentMousePos) && !ButtonPressed)
            {
                Dragging = true;
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                Dragging = false;
            }

            if (LastCoordinates != currentMousePos)
            {
                LastCoordinates = currentMousePos;
            }
        }

        #region Helper methods
        static IEnumerable<TileData<T>> ConvertTileData<T>(T[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    yield return new TileData<T> { Tile = arr[i, j], X = i, Y = j, };
                }
            }
        }

        struct TileData<T>
        {
            public T Tile;
            public int X, Y;
        }

        private void AddButtons()
        {
            WindowButtonControl zoomOut = new WindowButtonControl(WindowButtonControl.ButtonColor.Green, WindowButtonControl.OverlayType.Minus);
            zoomOut.Coords = new RelativeRectangle(new RelativeVector(0.0f, 9.0f), new RelativeVector(0.0f, 86.0f), 0.125f, 0.125f);
            AddChild(zoomOut);

            WindowButtonControl zoomIn = new WindowButtonControl(WindowButtonControl.ButtonColor.Green, WindowButtonControl.OverlayType.Plus);
            zoomIn.Coords = new RelativeRectangle(new RelativeVector(0.0f, 25.0f), new RelativeVector(0.0f, 102.0f), 0.125f, 0.125f);
            AddChild(zoomIn);

            WindowButtonControl close = new WindowButtonControl(WindowButtonControl.ButtonColor.Red, WindowButtonControl.OverlayType.Close);
            close.Coords = new RelativeRectangle(new RelativeVector(0.0f, 95.0f), new RelativeVector(0.0f, 17.0f), 0.125f, 0.125f);
            AddChild(close);
        }
        #endregion

        public void ButtonHandler(WindowButtonControl.OverlayType overlayType)
        {
            switch (overlayType)
            {
                case WindowButtonControl.OverlayType.Minus:
                    ZoomLevel = Math.Max(ZoomLevel - 1, 1);
                    break;
                case WindowButtonControl.OverlayType.Plus:
                    ZoomLevel = Math.Min(ZoomLevel + 1, 4);
                    break;
                case WindowButtonControl.OverlayType.Close:
                    Enabled = false;
                    break;
                default:
                    break;
            }
        }
    }
}
