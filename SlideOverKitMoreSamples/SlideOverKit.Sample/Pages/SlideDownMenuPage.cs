using System;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class SlideDownMenuPage : ContentPage,  IMenuContainerPage
    {
        // If your page cannot inheirt from the MenuContainerPage.
        // You can implement the IMenuContainerPage interface.
        // Overwrite these properties
        public Action HideMenuAction {            
            get;
            set;
        }

        public Action ShowMenuAction {
            get;
            set;
        }

        public SlideMenuView SlideMenu {
            get;
            set;
        }

        public SlideDownMenuPage ()
        {
            Content = new StackLayout {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    new Label { Text = "Click the button in top right" }
                }
            };

            // You can add a ToolBar button to show the Menu.
            this.ToolbarItems.Add (new ToolbarItem {
                Command = new Command (() => {
                    if (this.SlideMenu.IsShown) {
                        HideMenuAction?.Invoke ();
                    } else {
                        ShowMenuAction?.Invoke ();
                    }
                }),
                Icon = "Settings.png",
                Text = "Settings",
                Priority = 0
            });

            this.SlideMenu = new SlideDownMenuView ();
        }
    }
}


