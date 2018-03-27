using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public partial class PopOverView : SlidePopupView
    {
        public StackLayout StackContent => MyStackLayout;
        public PopOverView ()
        {
            InitializeComponent ();
            this.BackgroundColor = Color.White;
            // If you set Target attached property, you should not set TopMargin and LeftMargin
            // But you need to set Width and Height request
            this.WidthRequest = 200;
            this.HeightRequest = 300;          

        }
    }

    public class DeductibleModel
    {
        public DeductibleModel (double deductibleValue, double annualValue)
        {
            DeductibleValue = deductibleValue;
            AnnualValue = annualValue;

        }
        public double DeductibleValue {
            get; set;
        }

        public double AnnualValue {
            get; set;
        }
    }
}

