using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI
{
    public class MessageLog
    {

        public MessageLog(GraphicsDeviceManager graphics)
        {
            this._messageQueue = new Queue<string>();
            this._graphics = graphics;
            this.FadingBegins = 3000;
            this.FadingDuration = 2000;
            this.Visible = true;
        }

        /// <summary>
        /// Number of milliseconds until displayed words begin to fade away.
        /// </summary>
        public long FadingBegins { get; set; }

        /// <summary>
        /// Number of milliseconds fading lasts once begun.
        /// </summary>
        public long FadingDuration { get; set; }

        /// <summary>
        /// Whether or not the message log is visible.
        /// </summary>
        public bool Visible { get; set; }

        public void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            string[] messages = GetRecentMessages();

            Session.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
            for (int i = 0; i < messages.Length; i++)
            {
                if (_timer > FadingBegins)
                {
                    UIGraphic.DrawBorderText(Session.SpriteBatch,
                        UIGraphic.FiascoFontSmall,
                        messages[i],
                        _graphics.PreferredBackBufferWidth * 0.05f,
                        _graphics.PreferredBackBufferHeight * 0.70f + i * 23,
                        2.0f,
                        new Color(0.0f, 0.0f, 0.0f, MathHelper.Clamp(1 - (((_timer - FadingBegins) * 2) / (FadingDuration * 2f)), 0.0f, 1.0f)),
                        new Color(1.0f, 1.0f, 1.0f, MathHelper.Clamp(1 - (((_timer - FadingBegins) * 2) / (FadingDuration * 2f)), 0.0f, 1.0f)));
                }
                else
                {
                    UIGraphic.DrawBorderText(Session.SpriteBatch,
                    UIGraphic.FiascoFontSmall,
                    messages[i],
                    _graphics.PreferredBackBufferWidth * 0.05f,
                    _graphics.PreferredBackBufferHeight * 0.70f + i * 23,
                    2.0f);
                }
            }
            Session.SpriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime.Milliseconds;

            while (_messageQueue.Count > CAPACITY)
            {
                _messageQueue.Dequeue();
            }
        }

        #region Helper methods
        private string[] GetRecentMessages()
        {
            var queue = _messageQueue.ToArray();
            int numMessages = DISPLAYED_MESSAGES;
            if (_messageQueue.Count < DISPLAYED_MESSAGES)
            {
                numMessages = _messageQueue.Count;
            }

            string[] result = new string[numMessages];
            for (int i = 0; i < numMessages; i++)
            {
                result[i] = queue[_messageQueue.Count - 1 - i];
            }

            return result;
        }

        private void NotifyMessageAdded()
        {
            _timer = 0;
        }
        #endregion

        #region Message queue methods
        /// <summary>
        /// Adds the specified string to the Display Queue.
        /// </summary>
        /// <param name="text">String to add to queue.</param>
        public virtual void Enqueue(string text)
        {
            _messageQueue.Enqueue(text);
            NotifyMessageAdded();
        }

        /// <summary>
        /// Removes the specified string from the Display Queue.
        /// </summary>
        public void Dequeue()
        {
            _messageQueue.Dequeue();
        }
        #endregion

        private const int CAPACITY = 200;
        private const int DISPLAYED_MESSAGES = 5;
        private FiascoGame _game { get; set; }
        private GraphicsDeviceManager _graphics { get; set; }
        private long _timer { get; set; }
        private Queue<string> _messageQueue { get; set; }
    }
}
