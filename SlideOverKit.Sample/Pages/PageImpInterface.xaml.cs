using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SlideOverKit.Sample
{
    public partial class PageImpInterface : ContentPage,  IMenuContainerPage
    {
        public Action HideMenuAction {            
            get;
            set;
        }

        public Action ShowMenuAction {
            get;
            set;
        }

        public SlideMenuView SlideMenu {
            get;
            set;
        }

        public PageImpInterface ()
        {
            InitializeComponent ();
        }

        public PageImpInterface (MenuView menuview) : this ()
        {
            this.SlideMenu = menuview;
            menuview.NavigationPage = (orientation) => {
                Navigation.PushModalAsync (new MainPage (new MenuView (orientation){ IsFullScreen = true }));
            };
        }

        void ShowMenuClicked (object sender, EventArgs e)
        {
            if (ShowMenuAction != null)
                ShowMenuAction ();
        }

        void HideMenuClicked (object sender, EventArgs e)
        {
            if (HideMenuAction != null)
                HideMenuAction ();
        }
    }
}

