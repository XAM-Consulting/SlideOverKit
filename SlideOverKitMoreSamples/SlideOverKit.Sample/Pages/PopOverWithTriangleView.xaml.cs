using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public partial class PopOverWithTriangleView : SlideMenuView
    {
        public const int iOSTopMargin = 10;
        public const int AndroidTopMargin = 5;
        public const int WinPHoneTopMargin = 10;


        public PopOverWithTriangleView ()
        {
            InitializeComponent ();

            // In this case, you must set both LeftMargin and TopMarin

            this.LeftMargin = 10;

            //TODO we plan to add these properties
//            this.RightMargin = 10;
//            this.UseRightMargin = true;
//            this.UseLeftMargin = true;

            this.BackgroundViewColor = Color.Transparent;
            // In some small screen Android devices, the menu cannot be layout with full size.
            // In this case, we just set LeftMargin and TopMargin,
            // you do not need to set the HeightRequest or WidthRequest, as they are no effect in Pop up View.
            // If the view is too small for Android devices, we can reduce the TopMargin.
            this.TopMargin = Device.OnPlatform<int> (iOSTopMargin, AndroidTopMargin, WinPHoneTopMargin);

            // We can set the PopUpView coming directions.
            this.MenuOrientations = MenuOrientation.PopUpViewFromBottom;

            // The menu will hide without animation, 
            // If you want to have the animation, you can call the MenuContainerPage.HideMenu(), 
            // But you cannot call it from this View, cause of cycle references, you can sent a message to ContainerPage
            DoneButton.Clicked += (object sender, EventArgs e) => {
                this.HideWithoutAnimations ();
            };
        }
    }
}

