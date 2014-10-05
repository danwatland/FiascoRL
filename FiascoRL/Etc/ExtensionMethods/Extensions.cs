using FiascoRL.Display.UI.Controls;
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

    }
}
