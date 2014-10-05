using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Etc.GameState
{
    public sealed class GameStateManager : IGameState
    {
        private static GameStateManager _instance = null;
        private static readonly object _padlock = new Object();

        GameStateManager()
        {
            States.Add(new MainGameState());
            CurrentState = States[0];
        }

        /// <summary>
        /// Returns the game state manager.
        /// </summary>
        public static GameStateManager Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new GameStateManager();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Current GameState being used.
        /// </summary>
        public IGameState CurrentState { get; set; }

        /// <summary>
        /// List of GameStates this manager contains.
        /// </summary>
        public List<IGameState> States
        {
            get
            {
                if (_states == null)
                {
                    _states = new List<IGameState>();
                }
                return _states;
            }
            set
            {
                _states = value;
            }
        }
        private List<IGameState> _states;

        public void Initialize()
        {
            States.ForEach(x => x.Initialize());
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            CurrentState.Update(gameTime);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            CurrentState.Draw(gameTime);
        }
    }
}
