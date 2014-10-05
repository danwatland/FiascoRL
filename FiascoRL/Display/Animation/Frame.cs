using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.Animation
{
    public struct Frame
    {
        public Color Color { get; set; }
        public int GraphicIndex { get; set; }
        public string Text { get; set; }
        public Color BorderColor { get; set; }
        public Point Offset { get; set; }
        public float Size { get; set; }

    }
}
