using FiascoRL.Display.UI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI
{
    public class UIManager
    {
        /// <summary>
        /// Create a new UIManager.
        /// </summary>
        public UIManager()
        {
            this.Controls = new List<Control>();
        }

        /// <summary>
        /// List of controls managed by this UIManager.
        /// </summary>
        public List<Control> Controls { get; set; }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch the components should draw to.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Controls.Where(x => x.Enabled).ToList().ForEach(x => x.Draw(spriteBatch));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            Controls.Where(x => x.Enabled).ToList().ForEach(x => x.Update(gameTime));
        }
    }
}
