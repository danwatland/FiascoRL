using FiascoRL.Display.Animation;
using FiascoRL.Display.UI;
using FiascoRL.Entities;
using FiascoRL.Etc.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL
{
    public static class Session
    {
        /// <summary>
        /// Current player in this game.
        /// </summary>
        public static Player Player { get; set; }

        /// <summary>
        /// UIManager for this game.
        /// </summary>
        public static UIManager UIManager { get; set; }

        /// <summary>
        /// MessageLog for this game.
        /// </summary>
        public static MessageLog MessageLog { get; set; }

        /// <summary>
        /// List of animations for this game.
        /// </summary>
        public static List<Animation> Animations = new List<Animation>();

        /// <summary>
        /// List of text animations for this game.
        /// </summary>
        public static List<TextAnimation> TextAnimations = new List<TextAnimation>();

        /// <summary>
        /// Current game.
        /// </summary>
        public static FiascoGame Game { get; set; }

        /// <summary>
        /// SpriteBatch used to draw for this game.
        /// </summary>
        public static SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Game state manager for this game.
        /// </summary>
        public static GameStateManager GameStateManager { get { return GameStateManager.Instance; } }

        /// <summary>
        /// Graphics device manager for this game.
        /// </summary>
        public static GraphicsDevice Graphics { get; set; }
    }
}
