using System;
using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class PopOverPage: MenuContainerPage
    {
        public PopOverPage ()
        {
            var button = new Button {
                Text = "Show Menu",
                Command = new Command (() => {
                    this.ShowPopup ("FirstPopOver");
                })
            };
            this.PopupViews.Add ("FirstPopOver", new PopOverView () );

            Content = new StackLayout { 
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Children = {
                    button
                }
            };

            PopupViewAttached.SetTarget (button, "FirstPopOver");
        }
    }
}

