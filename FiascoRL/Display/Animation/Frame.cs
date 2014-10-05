using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.Animation
{
    public class Frame : ICloneable
    {
        public Color Color { get; set; }
        public int GraphicIndex { get; set; }
        public string Text { get; set; }
        public Color BorderColor { get; set; }
        public Point Offset { get; set; }
        public float Size { get; set; }

        public object Clone()
        {
            var frame = new Frame()
            {
                Color = this.Color,
                BorderColor = this.BorderColor,
                GraphicIndex = this.GraphicIndex,
                Offset = this.Offset,
                Size = this.Size,
                Text = this.Text,
            };
            return frame;
        }
    }
}
