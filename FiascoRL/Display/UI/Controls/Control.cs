using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Display.UI.Controls.Coordinates;
using Microsoft.Xna.Framework.Input;

namespace FiascoRL.Display.UI.Controls
{
    public abstract class Control
    {
        /// <summary>
        /// 2D UI Texture.
        /// </summary>
        public static Texture2D UITexture { set; get; }

        /// <summary>
        /// Graphics device manager for the window the game is running in.
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager { set; get; }

        /// <summary>
        /// Draws this control to the screen.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            }
            catch (InvalidOperationException e)
            {
                // Do nothing - SpriteBatch has already begun.
            }
            if (!CenterBitmap.IsEmpty)
            {
                Rectangle rect = GetActualCoords();
                // Check if all portions are defined.
                if (!NEBitmap.IsEmpty && !NBitmap.IsEmpty && !NWBitmap.IsEmpty && !WBitmap.IsEmpty &&
                    !SWBitmap.IsEmpty && !SBitmap.IsEmpty && !SWBitmap.IsEmpty && !EBitmap.IsEmpty)
                {
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X, rect.Y, NWBitmap.Width, NWBitmap.Height), NWBitmap, Color.White); // NW portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X, rect.Y + NWBitmap.Height, WBitmap.Width, rect.Height - NWBitmap.Height - SWBitmap.Height), WBitmap, Color.White); // W portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X, rect.Y + rect.Height - NWBitmap.Height, SWBitmap.Width, SWBitmap.Height), SWBitmap, Color.White); // SW portion

                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + NWBitmap.Width, rect.Y, rect.Width - NWBitmap.Width - NEBitmap.Width, NBitmap.Height), NBitmap, Color.White); // N portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + WBitmap.Width, rect.Y + NBitmap.Height, rect.Width - WBitmap.Width - EBitmap.Width, rect.Height - NBitmap.Height - SBitmap.Height), CenterBitmap, Color.White); // Center portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + SWBitmap.Width, rect.Y + rect.Height - NWBitmap.Height, rect.Width - SWBitmap.Width - SEBitmap.Width, SBitmap.Height), SBitmap, Color.White); // S portion

                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + rect.Width - NWBitmap.Width, rect.Y, NEBitmap.Width, NEBitmap.Height), NEBitmap, Color.White); // NE portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + rect.Width - NWBitmap.Width, rect.Y + NEBitmap.Height, EBitmap.Width, rect.Height - NEBitmap.Height - SEBitmap.Height), EBitmap, Color.White); // E portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + rect.Width - SWBitmap.Width, rect.Y + rect.Height - NEBitmap.Height, SEBitmap.Width, SEBitmap.Height), SEBitmap, Color.White); // SE portion
                }
                else if (!EBitmap.IsEmpty && !CenterBitmap.IsEmpty && !WBitmap.IsEmpty) // Check if this is a bar.
                {
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X, rect.Y, WBitmap.Width, WBitmap.Height), WBitmap, Color.White); // W portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + WBitmap.Width, rect.Y, rect.Width - WBitmap.Width - EBitmap.Width, CenterBitmap.Height), CenterBitmap, Color.White); // Center portion
                    spriteBatch.Draw(UITexture, new Rectangle(rect.X + rect.Width - EBitmap.Width, rect.Y, EBitmap.Width, EBitmap.Height), EBitmap, Color.White); // E portion
                }
                else // Just the center filled out.
                {
                    spriteBatch.Draw(UITexture, rect, CenterBitmap, Color.White);
                }

                if (Children != null)
                {
                    this.Children.ForEach(x => x.Draw(spriteBatch));
                }
            }
            else
            {
                throw new NullReferenceException("Center bitmap cannot be null.");
            }
        }

        /// <summary>
        /// Returns actual coordinates for this control, with all parent controls taken into account.
        /// </summary>
        /// <returns>Actual coordinates for this control.</returns>
        public Rectangle GetActualCoords()
        {
            if (this.Parent == null)
            {
                return new Rectangle((int)(GraphicsDeviceManager.PreferredBackBufferWidth * this.Coords.X.Position + this.Coords.X.Offset),
                                     (int)(GraphicsDeviceManager.PreferredBackBufferHeight * this.Coords.Y.Position + this.Coords.Y.Offset),
                                     (int)(GraphicsDeviceManager.PreferredBackBufferWidth * this.Coords.Width),
                                     (int)(GraphicsDeviceManager.PreferredBackBufferHeight * this.Coords.Height));
            }
            else
            {
                Rectangle parentCoords = this.Parent.GetActualCoords();
                return new Rectangle((int)(parentCoords.X + (parentCoords.Width) * this.Coords.X.Position + this.Coords.X.Offset),
                                     (int)(parentCoords.Y + (parentCoords.Height) * this.Coords.Y.Position + this.Coords.Y.Offset),
                                     (int)(parentCoords.Width * this.Coords.Width),
                                     (int)(parentCoords.Height * this.Coords.Height));
            }
        }

        /// <summary>
        /// Moves this control by the specified offsets.
        /// </summary>
        /// <param name="xDelta">Amount to move x-coordinate.</param>
        /// <param name="yDelta">Amount to move y-coordinate.</param>
        protected void Move(int xDelta, int yDelta)
        {
            this.Coords = new RelativeRectangle(new RelativeVector(Coords.X.Position, Coords.X.Offset + xDelta),
                                                new RelativeVector(Coords.Y.Position, Coords.Y.Offset + yDelta),
                                                Coords.Width,
                                                Coords.Height);
        }

        /// <summary>
        /// Adds a child control to this control.
        /// </summary>
        /// <param name="control">Control to add as child.</param>
        public void AddChild(Control control)
        {
            if (Children == null)
            {
                Children = new List<Control>();
            }

            Children.Add(control);
            control.Parent = this;
        }

        /// <summary>
        /// Parent of this control.
        /// </summary>
        public virtual Control Parent { get; set; }

        /// <summary>
        /// Coordinates of this control (relative to its parent).
        /// </summary>
        public RelativeRectangle Coords { get; set; }

        /// <summary>
        /// Bitmap representing the NW portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle NWBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the N portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle NBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the NE portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle NEBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the W portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle WBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the E portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle EBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the SW portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle SWBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the S portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle SBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the SE portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle SEBitmap { get; set; }

        /// <summary>
        /// Bitmap representing the center portion of the UI file used to draw this to the screen.
        /// </summary>
        public Rectangle CenterBitmap { get; set; }

        /// <summary>
        /// Whether or not this control is currently visible.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// All controls housed in this control.
        /// </summary>
        protected List<Control> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<Control>();
                }
                return _children;
            }
            set
            {
                _children = value;
            }

        }
        private List<Control> _children;
        /// <summary>
        /// Whether or not this control is currently being dragged.
        /// </summary>
        protected bool Dragging { get; set; }

        /// <summary>
        /// Whether or not a button is currently pressed.
        /// </summary>
        public bool ButtonPressed { get; set; }

        /// <summary>
        /// Coordinates of this control as of last update.
        /// </summary>
        protected Point LastCoordinates { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            this.Children.ForEach(x => x.Update(gameTime));
        }
    }
}
