using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public partial class PopOverView : SlidePopupView
    {
        public PopOverView ()
        {
            InitializeComponent ();
            this.BackgroundColor = Color.White;
            // If you set Target attached property, you should not set TopMargin and LeftMargin
            // But you need to set Width and Height request
            this.WidthRequest = 200;
            this.HeightRequest = 300;          

            DoneButton.Clicked += (object sender, EventArgs e) => {
                this.HideMySelf ();
            };

        }
    }
}

