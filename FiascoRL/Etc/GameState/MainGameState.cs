using FiascoRL.Display;
using FiascoRL.Entities;
using FiascoRL.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Etc.GameState
{
    public class MainGameState : IGameState
    {
        public void Initialize()
        {
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Session.Player.Update(gameTime);
            InputHandler.HandleInput(Session.Game, gameTime, Session.Player.CurrentLevel);

            // Update creatures.
            Session.Player.CurrentLevel.ActorList
                .Where(x => x.GetType() == typeof(Creature))
                .ToList()
                .ForEach(x => x.Update(gameTime));

            // Update animations.
            for (int i = Session.Animations.Count - 1; i >= 0; i--)
            {
                Session.Animations[i].Update(gameTime);
                if (Session.Animations[i].Completed)
                {
                    Session.Animations.Remove(Session.Animations[i]);
                }
            }

            // Update text animations.
            for (int i = Session.TextAnimations.Count - 1; i >= 0; i--)
            {
                Session.TextAnimations[i].Update(gameTime);
                if (Session.TextAnimations[i].Completed)
                {
                    Session.TextAnimations.Remove(Session.TextAnimations[i]);
                }
            }

            Session.UIManager.Update(gameTime);
            Session.MessageLog.Update(gameTime);
        }

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Session.Graphics.Clear(Color.Black);
            Camera2D cam = new Camera2D();
            cam.Zoom = 2f;
            cam.Pos = new Vector2(Session.Player.Coords.X * 24, Session.Player.Coords.Y * 24); // Place on Session.Player's coordinates.
            Matrix transformation = cam.get_transformation(Session.Graphics);

            DrawingManager.StoreVisibleTiles();
            DrawingManager.DrawTiles(gameTime, transformation);
            DrawingManager.DrawDecorations(gameTime, transformation);
            DrawingManager.DrawTarget(gameTime, transformation);
            DrawingManager.DrawItems(gameTime, transformation);
            DrawingManager.DrawCreatures(gameTime, transformation);
            DrawingManager.DrawPlayer(gameTime, transformation);
            DrawingManager.DrawAnimations(gameTime, transformation);
            DrawingManager.DrawHealthBars(gameTime, transformation, Session.Game);

            // Draw UI.
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null);
            Session.UIManager.Draw(Session.SpriteBatch);
            Session.SpriteBatch.End();

            Session.MessageLog.Draw(gameTime);
        }
    }
}
