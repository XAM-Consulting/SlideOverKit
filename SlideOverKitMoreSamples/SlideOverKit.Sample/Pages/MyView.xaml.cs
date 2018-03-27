using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public partial class MyView : ContentView
    {
        public MyView ()
        {
            InitializeComponent ();
            MainList.ItemsSource = new List<DeductibleModel> { new DeductibleModel (0, 75), new DeductibleModel (100, 75), new DeductibleModel (250, 71), new DeductibleModel (500, 63) };
        }
    }
}
