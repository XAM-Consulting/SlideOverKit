using System;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class MainPage : ContentPage
    {
        public MainPage ()
        {
            Content = new StackLayout { 
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center, 
                Spacing = 10,
                Children = {
                    new Button {
                        Text = "RightSideMasterDetail",
                        Command = LaunchRightSideMasterDetailPage,
                    },
                    new Button {
                        Text = "SlideUpMenu",
                        Command = LaunchSlideUpMenuPage,
                    },
                    new Button{
                        Text ="SlideDownMenu",
                        Command = LaunchSlideDownMenuPage,
                    },
                    new Button{
                        Text ="QuickInnerMenu",
                        Command = LaunchQuickInnerMenuPage,
                    },
                    new Button{
                        Text ="Popovers",
                        Command = LaunchPopoversPage,
                    }
                }
            };
        }

        public Command LaunchRightSideMasterDetailPage {
            get {
                return new Command (() => {
                    Navigation.PushAsync(new RightSideDetailPage());
                });
            }
        }

        public Command LaunchSlideUpMenuPage {
            get {
                return new Command (() => {
                    Navigation.PushAsync(new SlideUpMenuPage());
                });
            }
        }

        public Command LaunchSlideDownMenuPage{
            get{
                return new Command (() => {
                    Navigation.PushAsync(new SlideDownMenuPage());
                });
            }
        }

        public Command LaunchQuickInnerMenuPage{
            get{
                return new Command (() => {
                    Navigation.PushAsync(new QuickInnerMenuPage());
                });
            }
        }

        public Command LaunchPopoversPage{
            get{
                return new Command (() => {
                    Navigation.PushAsync(new PopOverPage());
                });
            }
        }              
    }
}


