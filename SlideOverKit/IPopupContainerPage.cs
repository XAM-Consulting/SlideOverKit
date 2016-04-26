using System;
using System.Collections.Generic;

namespace SlideOverKit
{
    public interface IPopupContainerPage
    {
        Dictionary<string, SlidePopupView> PopupViews { get; set; }

        Action<string>  ShowPopupAction { get; set; }

        Action HidePopupAction { get; set; }
    }
}

