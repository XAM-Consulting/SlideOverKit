using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public partial class SlideUpMenuView : SlideMenuView
    {
        public SlideUpMenuView ()
        {
            InitializeComponent ();
            // You must set HeightRequest in this case
            this.HeightRequest = 250;
            // You must set IsFullScreen in this case, 
            // otherwise you need to set WidthRequest, 
            // just like the QuickInnerMenu sample
            this.IsFullScreen = true;
            this.MenuOrientations = MenuOrientation.BottomToTop;

            // You must set BackgroundColor, 
            // and you cannot put another layout with background color cover the whole View
            // otherwise, it cannot be dragged on Android
            this.BackgroundColor = Color.Black;
            this.BackgroundViewColor = Color.Transparent;

            // In some small screen size devices, the menu cannot be full size layout.
            // In this case we need to set different size for Android.
            if (Device.RuntimePlatform == Device.Android)
                this.HeightRequest += 50;
        }
    }
}

