using System;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class QuickInnerMenuView : SlideMenuView
    {
        public QuickInnerMenuView (MenuOrientation orientation)
        {
            var mainLayout = new StackLayout {
                Spacing = 15,
                Children = {
                    new Image {
                        Source = "Happy.png",
                        WidthRequest = 25,
                        HeightRequest = 25,
                    },
                    new Image {
                        Source = "Home.png",
                        WidthRequest = 25,
                        HeightRequest = 25,
                    },
                    new Image {
                        Source = "MessageFilled.png",
                        WidthRequest = 25,
                        HeightRequest = 25,
                    },
                    new Image {
                        Source = "Settings.png",
                        WidthRequest = 25,
                        HeightRequest = 25,
                    },
                }
            };
            // In this case the IsFullScreen must set false
            this.IsFullScreen = false;
            this.BackgroundViewColor = Color.Transparent;

            // You must set BackgroundColor, 
            // and you cannot put another layout with background color cover the whole View
            // otherwise, it cannot be dragged on Android
            this.BackgroundColor = Color.FromHex ("#C82630");
            this.MenuOrientations = orientation;
            if (orientation == MenuOrientation.BottomToTop) {
                mainLayout.Orientation = StackOrientation.Vertical;
                mainLayout.Children.Insert (0, new Image {
                    Source = "DoubleUp.png",
                    WidthRequest = 25,
                    HeightRequest = 25,
                });
                mainLayout.Padding = new Thickness (0, 5);
                // In this case, you must set both WidthRequest and HeightRequest.
                this.WidthRequest = 50; 
                this.HeightRequest = 200;

                // A little bigger then DoubleUp.png image size, used for user drag it.
                this.DraggerButtonHeight = 30;

                // In this menu direction you must set LeftMargin.
                this.LeftMargin = 100;

            }

            if (orientation == MenuOrientation.TopToBottom) {
                mainLayout.Orientation = StackOrientation.Vertical;
                mainLayout.Children.Insert (4, new Image {
                    Source = "DoubleDown_White.png",
                    WidthRequest = 25,
                    HeightRequest = 25,
                });
                mainLayout.Padding = new Thickness (0, 5);
                this.WidthRequest = 50;
                this.HeightRequest = 200;
                this.DraggerButtonHeight = 40;
                this.LeftMargin = 100;

            }

            if (orientation == MenuOrientation.LeftToRight) {
                mainLayout.Orientation = StackOrientation.Horizontal;
                mainLayout.Children.Insert (4, new Image {
                    Source = "DoubleRight.png",
                    WidthRequest = 25,
                    HeightRequest = 25,
                });
                mainLayout.Padding = new Thickness (5, 0);
                this.WidthRequest = 200;
                this.HeightRequest = 50;
                // In this case, it should be DraggerButtonWidth not DraggerButtonHeight
                this.DraggerButtonWidth = 40;

                // In this menu direction you must set TopMargin.
                this.TopMargin = 30;

            }

            if (orientation == MenuOrientation.RightToLeft) {
                mainLayout.Orientation = StackOrientation.Horizontal;
                mainLayout.Children.Insert (0, new Image {
                    Source = "DoubleLeft.png",
                    WidthRequest = 25,
                    HeightRequest = 25,
                });
                mainLayout.Padding = new Thickness (5, 0);
                this.WidthRequest = 200;
                this.HeightRequest = 50;
                this.DraggerButtonWidth = 30;
                this.TopMargin = 30;
            }
            Content = mainLayout;
        }
    }
}


