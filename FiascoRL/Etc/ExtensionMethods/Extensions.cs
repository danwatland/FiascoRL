using FiascoRL.Display.UI.Controls;
using FiascoRL.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Etc.ExtensionMethods
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the UI component of the specified type.
        /// </summary>
        /// <param name="source">IEnumerable containing UI objects.</param>
        /// <param name="type">Type of component to return.</param>
        /// <returns>UI component of the specified type.</returns>
        public static Control GetUIComponent(this IEnumerable<Control> source, Type type)
        {
            var result = source.Where(x => x.GetType() == type);
            return result.First();
        }

        public static void PerformAction(this Tile[,] tileMap, Func<Tile, Tile> func)
        {
            for (int x = 0; x < tileMap.GetLength(0); x++)
            {
                for (int y = 0; y < tileMap.GetLength(1); y++)
                {
                    tileMap[x, y] = func(tileMap[x, y]);
                }
            }
        }
    }
}
