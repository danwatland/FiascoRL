using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls.Interfaces
{
    public interface IHoverableIconHandler
    {
        /// <summary>
        /// Deselects all icons.
        /// </summary>
        void DeselectAll();

        /// <summary>
        /// Returns selected icon.
        /// </summary>
        HoverableIconControl GetSelected();
    }
}
