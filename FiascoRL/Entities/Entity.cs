using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FiascoRL.Entities
{
    /// <summary>
    /// Representation of an object to be drawn to the screen.
    /// </summary>
    public abstract class Entity
    {
        public Texture2D Texture { get; set; }

        public int GraphicIndex { get; set; }

        /// <summary>
        /// Create a new blank entity.
        /// </summary>
        public Entity() {}

        /// <summary>
        /// Create new entity with the specified texture and graphic index.
        /// </summary>
        /// <param name="texture">Texture to pull this entity's image from.</param>
        /// <param name="graphicIndex">Image to pull.</param>
        public Entity(Texture2D texture, int graphicIndex)
        {
            this.Texture = texture;
            this.GraphicIndex = graphicIndex;
        }

        /// <summary>
        /// Updates this entity's animation state.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);
    }
}
