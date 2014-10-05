using FiascoRL.Display.Animation;
using FiascoRL.Display.UI;
using FiascoRL.Entities;
using FiascoRL.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display
{
    public static class DrawingManager
    {
        private static List<Tile> _visibleTiles;

        public static void StoreVisibleTiles()
        {
            _visibleTiles = Session.Player.LOS.GetVisible();
        }

        public static void DrawTiles(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            for (int x = 0; x < Session.Player.CurrentLevel.Width; x++)
            {
                for (int y = 0; y < Session.Player.CurrentLevel.Height; y++)
                {
                    if (_visibleTiles.Contains(Session.Player.CurrentLevel.TileMap[x, y]))
                    {
                        Session.SpriteBatch.Draw(Session.Player.CurrentLevel.LevelTexture, new Rectangle(x * 24, y * 24, 24, 24),
                            SpriteGraphic.GetSprite(SpriteGraphic.World, Session.Player.CurrentLevel.TileMap[x, y].GraphicIndex), Color.White);
                    }
                    else if (Session.Player.CurrentLevel.TileMap[x, y].TurnSeen >= 0)
                    {
                        Session.SpriteBatch.Draw(Session.Player.CurrentLevel.LevelTexture, new Rectangle(x * 24, y * 24, 24, 24),
                            SpriteGraphic.GetSprite(SpriteGraphic.World, Session.Player.CurrentLevel.TileMap[x, y].GraphicIndex), Color.Gray);
                    }

                }
            }

            Session.SpriteBatch.End();
        }

        public static void DrawDecorations(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            for (int x = 0; x < Session.Player.CurrentLevel.Width; x++)
            {
                for (int y = 0; y < Session.Player.CurrentLevel.Height; y++)
                {
                    if (Session.Player.CurrentLevel.DecorationMap[x, y] != null && _visibleTiles.Contains(Session.Player.CurrentLevel.TileMap[x, y]))
                    {
                        Session.SpriteBatch.Draw(Session.Player.CurrentLevel.LevelTexture, new Rectangle(x * 24, y * 24, 24, 24),
                            SpriteGraphic.GetSprite(SpriteGraphic.World, Session.Player.CurrentLevel.DecorationMap[x, y].GraphicIndex), Color.White);
                    }
                }
            }

            Session.SpriteBatch.End();
        }

        public static void DrawTarget(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, transformation);
            Session.Player.Target.Draw(Session.SpriteBatch);
            Session.SpriteBatch.End();
        }

        public static void DrawItems(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            // Draw items in level.
            Session.Player.CurrentLevel.ActorList.Where(x => x.GetType() == typeof(Item))
                .Where(x => Session.Player.CurrentLevel.TileMap[x.Coords.X, x.Coords.Y].TurnSeen == Session.Player.CurrentTurn)
                .ToList()
                .ForEach(x => Session.SpriteBatch.Draw(x.Texture, new Rectangle(x.Coords.X * 24 + 4, x.Coords.Y * 24 + 4, 16, 16), SpriteGraphic.GetSprite(x.Texture, x.GraphicIndex), Color.White));
            Session.SpriteBatch.End();
        }

        public static void DrawCreatures(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            // Draw items in level.
            Session.Player.CurrentLevel.ActorList.Where(x => x.GetType() == typeof(Creature))
                .Where(x => Session.Player.CurrentLevel.TileMap[x.Coords.X, x.Coords.Y].TurnSeen == Session.Player.CurrentTurn)
                .ToList()
                .ForEach(x =>
                {
                    Session.SpriteBatch.Draw(x.Texture, new Rectangle(x.Coords.X * 24, x.Coords.Y * 24, 24, 24), SpriteGraphic.GetSprite(x.Texture, ((Creature)x).ShadowIndex), Color.White);
                    Session.SpriteBatch.Draw(x.Texture, new Rectangle(x.Coords.X * 24, x.Coords.Y * 24 - 2, 24, 24), SpriteGraphic.GetSprite(x.Texture, x.GraphicIndex), Color.White);
                });

            Session.SpriteBatch.End();
        }

        public static void DrawPlayer(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            Session.SpriteBatch.Draw(SpriteGraphic.Creatures, new Rectangle(Session.Player.Coords.X * 24, Session.Player.Coords.Y * 24, 24, 24), SpriteGraphic.GetSprite(SpriteGraphic.Creatures, 401), Color.White);
            Session.SpriteBatch.Draw(SpriteGraphic.Creatures, new Rectangle(Session.Player.Coords.X * 24, Session.Player.Coords.Y * 24 - 2, 24, 24), SpriteGraphic.GetSprite(SpriteGraphic.Creatures, Session.Player.GraphicIndex), Color.White);

            Session.SpriteBatch.End();
        }

        public static void DrawAnimations(GameTime gameTime, Matrix transformation)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            foreach (Display.Animation.Animation a in Session.Animations)
            {
                Session.SpriteBatch.Draw(a.Texture, new Rectangle(a.Coords.X * 24 + a.Offset.X, a.Coords.Y * 24 + a.Offset.Y, 24, 24), SpriteGraphic.GetSprite(a.Texture, a.GraphicIndex), a.CurrentColor);
            }
            foreach (TextAnimation a in Session.TextAnimations)
            {
                UIGraphic.DrawBorderText(Session.SpriteBatch, a.Font, a.Text, a.Coords.X * 24 + a.Offset.X, a.Coords.Y * 24 + a.Offset.Y, a.Size, a.BorderColor, a.TextColor);
            }

            Session.SpriteBatch.End();
        }

        public static void DrawHealthBars(GameTime gameTime, Matrix transformation, FiascoGame game)
        {
            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, transformation);

            Session.Player.CurrentLevel.ActorList.Where(x => x.GetType() == typeof(Creature))
                .Cast<Creature>()
                .ToList()
                .ForEach(x => SpriteGraphic.DrawHealthBar(Session.SpriteBatch, game, x));

            Session.SpriteBatch.End();
        }


    }
}
