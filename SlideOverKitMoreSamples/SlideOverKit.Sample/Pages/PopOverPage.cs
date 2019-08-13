using System;
using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class PopOverPage : MenuContainerPage
    {        
        public PopOverPage ()
        {
            var button = new Button {
                Text = "Show First Popup",
                Command = new Command (() => 
                {                    
                    this.ShowPopup ("FirstPopup");
                })
            };

            // we can add two more Popup control in this way
            this.PopupViews.Add ("FirstPopup", new PopOverView ());
            this.PopupViews.Add ("SecondPopup", new PopOverWithTriangleView ());


            Content = new ScrollView {
                Orientation = ScrollOrientation.Both,
                Content = new StackLayout {
                    Spacing = 10,
                    Children = {
                        new BoxView {BackgroundColor = Color.Transparent, HeightRequest = 300, WidthRequest=300},
                        button,
                        new BoxView {BackgroundColor = Color.Transparent, HeightRequest = 500, WidthRequest=500},
                        }
                }
            };

            this.ToolbarItems.Add (new ToolbarItem {
                Command = new Command (() => {
                    
                    if (this.PopupViews ["SecondPopup"].IsShown) {
                        this.HidePopup ();
                    } else {
                        this.ShowPopup ("SecondPopup");
                    }
                }),
                Icon = "Filter_Blue.png",
                Text = "Filter",
                Priority = 0
            });

            // Set First popup target control as the button
            // If you set target control you must set WidthRequest and HeightRequest in PopUpView
            PopupViewAttached.SetTarget (button, "FirstPopup");
            // Set Second popup without target
            // In this case, you must set LeftMargin and TopMargin
        }
    }
}

