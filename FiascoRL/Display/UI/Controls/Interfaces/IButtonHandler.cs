using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiascoRL.Display.UI.Controls.Interfaces
{
    public interface IButtonHandler
    {
        /// <summary>
        /// Contains logic to handle the pressing of any buttons within this control.
        /// </summary>
        /// <param name="overlayType">Type of button.</param>
        void ButtonHandler(WindowButtonControl.OverlayType overlayType);
    }
}
