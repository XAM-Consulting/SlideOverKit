using System;

using Xamarin.Forms;

namespace SlideOverKit
{
    public class MenuContainerPage : ContentPage, IMenuContainerPage
    {
        public MenuContainerPage ()
        {
        }

        public SlideMenuView SlideMenu { get; set; }

        public Action ShowMenuAction { get; set; }

        public Action HideMenuAction { get; set; }

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
    }
}


