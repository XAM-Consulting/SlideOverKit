using System;

namespace SlideOverKit
{
    public interface IMenuContainerPage
    {
        SlideMenuView SlideMenu { get; set; }

        Action ShowMenuAction { get; set; }

        Action HideMenuAction { get; set; }
    }
}

