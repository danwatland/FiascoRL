using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FiascoRL.Display.Animation;
using FiascoRL.World;

namespace FiascoRL.Entities
{
    /// <summary>
    /// Representation of an entity that can interact with the environment.
    /// </summary>
    public class Actor : Entity
    {
        private int _graphicIndexA, _graphicIndexB;
        private float _frameTimer;
        private static readonly float _frameLength = 0.35f;

        /// <summary>
        /// X and Y-coordinates of this actor.
        /// </summary>
        public Point Coords { get; set; }

        /// <summary>
        /// Level this actor is currently on.
        /// </summary>
        public Level CurrentLevel { get; set; }

        /// <summary>
        /// Create a new blank actor.
        /// </summary>
        public Actor()
        {

        }

        /// <summary>
        /// Create a new actor with the specified texture and graphic index.
        /// </summary>
        /// <param name="texture">Texture to pull this actor's image from.</param>
        /// <param name="graphicIndex">Image to pull.</param>
        public Actor(Texture2D texture, int graphicIndex)
            : base(texture, graphicIndex)
        {
            this._graphicIndexA = graphicIndex;
            this._graphicIndexB = graphicIndex + 18;
        }

        /// <summary>
        /// Changes this actor's graphic index.
        /// </summary>
        /// <param name="index">Image to pull.</param>
        public void ChangeGraphicIndex(int index)
        {
            this._graphicIndexA = index;
            this._graphicIndexB = index + 18;
        }

        /// <summary>
        /// Current animation in effect for this creature.
        /// </summary>
        public Animation Animation { get; set; }

        /// <summary>
        /// Current color for this creature.
        /// </summary>
        public Color Color { get; set; }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameTimer > _frameLength)
            {
                _frameTimer = 0.0f;
                GraphicIndex = (GraphicIndex == _graphicIndexA) ? _graphicIndexB : _graphicIndexA;
            }
        }
    }
}
