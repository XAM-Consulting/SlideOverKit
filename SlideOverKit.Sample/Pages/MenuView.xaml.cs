using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SlideOverKit.Sample
{
    public partial class MenuView : SlideMenuView
    {
        public Action<MenuOrientation> NavigationPage { get; set; }

        public MenuView ()
        {
            InitializeComponent ();

            MenuFromTopLayout.GestureRecognizers.Add (new TapGestureRecognizer { Command = new Command (() => {
                    if (NavigationPage != null)
                        NavigationPage (MenuOrientation.TopToBottom);                
                })
            });

            MenuFromBottomLayout.GestureRecognizers.Add (new TapGestureRecognizer { Command = new Command (() => {
                    if (NavigationPage != null)
                        NavigationPage (MenuOrientation.BottomToTop);                
                })
            });
        
            MenuFromLeftLayout.GestureRecognizers.Add (new TapGestureRecognizer { Command = new Command (() => {
                    if (NavigationPage != null)
                        NavigationPage (MenuOrientation.LeftToRight);                
                })
            });

            MenuFromRightLayout.GestureRecognizers.Add (new TapGestureRecognizer { Command = new Command (() => {
                    if (NavigationPage != null)
                        NavigationPage (MenuOrientation.RightToLeft);                
                })
            });
            HideButton.Clicked+= (object sender, EventArgs e) => {
                this.HideWithoutAnimations ();
            };
        }

        public MenuView (MenuOrientation orientation) : this ()
        {
            this.MenuOrientations = orientation;
            // Vertical Menu
            if (this.MenuOrientations == MenuOrientation.TopToBottom) {               
                // Vertical menu only need leftMargin
                // Full size Menu 
                this.LeftMargin = 0;
                this.IsFullScreen = true;
                this.DraggerButtonHeight = 0;
            }

            if (this.MenuOrientations == MenuOrientation.BottomToTop) {
                // Not Full size
                this.LeftMargin = 0;
                this.IsFullScreen = true;
                //this.MainLayout.Children.Insert (0, this.DragImage);
                this.DraggerButtonHeight = 0;
            }

            // Horizontal Menu
            if (this.MenuOrientations == MenuOrientation.LeftToRight) {

                // Horizaontal menu only need topMargin
                // Full size Menu 
                this.TopMargin = 0;
                this.IsFullScreen = true;           
                this.DraggerButtonWidth = 0;
            }
            if (this.MenuOrientations == MenuOrientation.RightToLeft) {
                // Not Full size
                this.TopMargin = 0;
                this.IsFullScreen = true;
                //this.MainLayout.Children.RemoveAt (1);
                //this.MainLayout.Children.Insert (0, this.DragImage);
                this.DraggerButtonWidth = 30;
            }
        }
    }
}

