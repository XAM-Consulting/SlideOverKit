using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public partial class SlideDownMenuView : SlideMenuView
    {
        public SlideDownMenuView ()
        {
            InitializeComponent ();

            // You must set HeightRequest in this case
            this.HeightRequest = 600;
            // You must set IsFullScreen in this case, 
            // otherwise you need to set WidthRequest, 
            // just like the QuickInnerMenu sample
            this.IsFullScreen = true;
            this.MenuOrientations = MenuOrientation.TopToBottom;

            // You must set BackgroundColor, 
            // and you cannot put another layout with background color cover the whole View
            // otherwise, it cannot be dragged on Android
            this.BackgroundColor = Color.FromHex ("#D8DDE7");         
        }
    }
}

