using System;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class PopOverWithTrianglePage : MenuContainerPage
    {
        public PopOverWithTrianglePage ()
        {
            Content = new StackLayout {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    new Label { 
                        Text = "Click the button in top right",
                        TextColor = Color.White
                    }
                }
            };

            this.BackgroundImage = "Map_Background.png";

            this.ToolbarItems.Add (new ToolbarItem {
                Command = new Command (() => {
                    this.ShowMenu ();
                }),
                Icon = "Filter_Blue.png",
                Text = "Filter",
                Priority = 0
            });

            this.SlideMenu = new PopOverWithTriangleView ();

        }
    }
}


