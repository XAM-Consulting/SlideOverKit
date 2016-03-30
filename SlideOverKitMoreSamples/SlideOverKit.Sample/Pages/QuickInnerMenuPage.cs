using System;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class QuickInnerMenuPage : MenuContainerPage
    {
        public QuickInnerMenuPage ()
        {
            Content = new StackLayout { 
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    new Button { Text = "Quick menu from right", Command = ShownMenuFromRight },
                    new Button { Text = "Quick menu from left", Command = ShownMenuFromLeft },  
                    new Button { Text = "Quick menu from top", Command = ShownMenuFromTop },
                    new Button { Text = "Quick menu from bottom", Command = ShownMenuFromBottom },  
                    new Button { Text = "Go Back to Main page", Command = GoBackToMainPage }
                }
            };

            this.SlideMenu = new QuickInnerMenuView (MenuOrientation.RightToLeft);
        }

        public Command ShownMenuFromRight {
            get {
                return new Command (() => {
                    Navigation.PushModalAsync (new QuickInnerMenuPage {
                        SlideMenu = new QuickInnerMenuView (MenuOrientation.RightToLeft)
                    });
                });
            }
        }

        public Command ShownMenuFromLeft {
            get {
                return new Command (() => {
                    Navigation.PushModalAsync (new QuickInnerMenuPage {
                        SlideMenu = new QuickInnerMenuView (MenuOrientation.LeftToRight)
                    });
                });
            }
        }

        public Command ShownMenuFromTop {
            get {
                return new Command (() => {
                    Navigation.PushModalAsync (new QuickInnerMenuPage {
                        SlideMenu = new QuickInnerMenuView (MenuOrientation.TopToBottom)
                    });
                });
            }
        }

        public Command ShownMenuFromBottom {
            get {
                return new Command (() => {
                    Navigation.PushModalAsync (new QuickInnerMenuPage {
                        SlideMenu = new QuickInnerMenuView (MenuOrientation.BottomToTop)
                    });
                });
            }
        }

        public Command GoBackToMainPage {
            get {
                return new Command ((obj) => {
                    MessagingCenter.Send<SlideOverKit.MoreSample.QuickInnerMenuPage> (this, "GoBackToMainPage");
                });
            }
        }
    }
}


