using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace SlideOverKit
{
    public class MenuContainerPage : ContentPage, IMenuContainerPage, IPopupContainerPage
    {
        public MenuContainerPage ()
        {
            PopupViews = new Dictionary<string, SlidePopupView> ();
        }

        public SlideMenuView SlideMenu { get; set; }

        public Action ShowMenuAction { get; set; }

        public Action HideMenuAction { get; set; }

        public Dictionary<string, SlidePopupView> PopupViews { get; set; }

        public Action<string>  ShowPopupAction { get; set; }

        public Action HidePopupAction { get; set; }

        public void ShowMenu ()
        {
            if (ShowMenuAction != null)
                ShowMenuAction ();
        }

        public void HideMenu ()
        {
            if (HideMenuAction != null)
                HideMenuAction ();
        }

        public void ShowPopup (string name)
        {
            if (ShowPopupAction != null)
                ShowPopupAction (name);
        }

        public void HidePopup ()
        {
            if (HidePopupAction != null)
                HidePopupAction ();
        }
    }
}


