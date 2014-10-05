using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FiascoRL.Display.Animation
{
    public static class AnimationExtensions
    {
        public static void AddLinearFade(this Animation animation)
        {
            animation.AddLinearFade(0, animation.Frames.Count - 1);
        }

        public static void AddLinearFade(this Animation animation, int startFrame, int endFrame)
        {
            if (startFrame < 0 || startFrame >= animation.Frames.Count() || endFrame < 0 || endFrame >= animation.Frames.Count() || startFrame > endFrame)
                throw new IndexOutOfRangeException();
            for (int i = startFrame; i < endFrame; i++)
            {
                var frameColor = animation.Frames[i].Color;
                animation.Frames[i].Color = new Color(frameColor, (i - startFrame) / (float)(endFrame - startFrame));
            }
        }

        public static IEnumerable<Frame> CreateClonedEnumerable(this Frame frame, int n)
        {
            var frameList = new List<Frame>();
            for (int i = 0; i < n; i++)
            {
                frameList.Add((Frame)frame.Clone());
            }
            return frameList;
        }

        public static void SetGraphicIndexes(this Animation animation, int index)
        {
            animation.Frames.ForEach(f => f.GraphicIndex = index);
        }

        public static void SetGraphicIndexes(this Animation animation, IEnumerable<int> indexes)
        {
            if (animation.Frames.Count() != indexes.Count())
                throw new IndexOutOfRangeException("Frame and index enumerables are not the same size.");
            for (int i = 0; i < animation.Frames.Count(); i++)
            {
                animation.Frames[i].GraphicIndex = indexes.ElementAt(i);
            }
        }

        public static void SetColors(this Animation animation, Color color)
        {
            animation.SetColors(color, 0, animation.Frames.Count() - 1);
        }

        public static void SetColors(this Animation animation, Color color, int startIndex, int endIndex)
        {
            if (startIndex < 0 || startIndex >= animation.Frames.Count() || endIndex < 0 || endIndex >= animation.Frames.Count() || startIndex > endIndex)
                throw new IndexOutOfRangeException();
            for (int i = startIndex; i <= endIndex; i++)
            {
                animation.Frames[i].Color = color;
            }
        }
    }
}
