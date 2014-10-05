using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiascoRL.Display.UI.Controls.Coordinates;
using FiascoRL.Display.UI.Controls.Interfaces;

namespace FiascoRL.Display.UI.Controls
{
    /// <summary>
    /// Basis for any control that uses a window to house information.
    /// </summary>
    public abstract class WindowControl : Control, IButtonHandler
    {
        /// <summary>
        /// Creates a new blank window.
        /// </summary>
        public WindowControl()
        {
            this.NWBitmap = new Microsoft.Xna.Framework.Rectangle(16, 40, 32, 32);
            this.NBitmap = new Microsoft.Xna.Framework.Rectangle(49, 40, 32, 32);
            this.NEBitmap = new Microsoft.Xna.Framework.Rectangle(82, 40, 32, 32);
            this.WBitmap = new Microsoft.Xna.Framework.Rectangle(478, 24, 32, 32);
            this.CenterBitmap = new Microsoft.Xna.Framework.Rectangle(511, 24, 32, 32);
            this.EBitmap = new Microsoft.Xna.Framework.Rectangle(544, 24, 32, 32);
            this.SWBitmap = new Microsoft.Xna.Framework.Rectangle(478, 80, 32, 32);
            this.SBitmap = new Microsoft.Xna.Framework.Rectangle(511, 80, 32, 32);
            this.SEBitmap = new Microsoft.Xna.Framework.Rectangle(544, 80, 32, 32);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.Children.ForEach(x => x.Update(gameTime));
        }

        public void ButtonHandler(WindowButtonControl.OverlayType overlayType)
        {
            if (overlayType == WindowButtonControl.OverlayType.Close)
            {
                this.Enabled = false;
            }
        }
    }
}
