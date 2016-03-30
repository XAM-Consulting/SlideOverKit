using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SlideOverKit.Sample
{
	public partial class MainPage : MenuContainerPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

        public MainPage(MenuView menuview): this()
        {
            this.SlideMenu = menuview;
            menuview.NavigationPage = (orientation)=>{
                Navigation.PushModalAsync(new MainPage (new MenuView (orientation){IsFullScreen = true}));
                    };
        }

		void ShowMenuClicked(object sender, EventArgs e)
		{
			this.ShowMenu ();
		}

		void HideMenuClicked(object sender, EventArgs e)
		{
			this.HideMenu ();
		}
	}
}

