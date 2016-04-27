using System;

using Xamarin.Forms;

namespace SlideOverKit
{
    [Flags]
    public enum MenuOrientation
    {
        TopToBottom,
        BottomToTop,
        LeftToRight,
        RightToLeft,
    }

    public class SlideMenuView : ContentView
    {
        public SlideMenuView ()
        {
            // It must set background color, otherwise it will cannot be dragged in Android 
            this.BackgroundColor = Color.White;
        }

        public static readonly BindableProperty MenuOrientationsProperty = BindableProperty.Create (
                                                                               "MenuOrientations", 
                                                                               typeof(MenuOrientation), 
                                                                               typeof(SlideMenuView), 
                                                                               MenuOrientation.TopToBottom);

        public MenuOrientation MenuOrientations { 
            get {
                return (MenuOrientation)GetValue (MenuOrientationsProperty);
            }
            set {
                SetValue (MenuOrientationsProperty, value);
            }
        }

        public static readonly BindableProperty LeftMarginProperty = BindableProperty.Create (
                                                                         "LeftMargin",
                                                                         typeof(double),
                                                                         typeof(SlideMenuView),
                                                                         0.0);

        public double LeftMargin { 
            get {
                return (double)GetValue (LeftMarginProperty);
            }
            set {
                SetValue (LeftMarginProperty, value);
            }
        }

        public static readonly BindableProperty TopMarginProperty = BindableProperty.Create (
                                                                        "TopMargin",
                                                                        typeof(double),
                                                                        typeof(SlideMenuView),
                                                                        0.0);

        public double TopMargin { 
            get {
                return (double)GetValue (TopMarginProperty);
            }
            set {
                SetValue (TopMarginProperty, value);
            }
        }

        public static readonly BindableProperty DraggerButtonHeightProperty = BindableProperty.Create (
                                                                                  "DraggerButtonHeight",
                                                                                  typeof(double),
                                                                                  typeof(SlideMenuView),
                                                                                  0.0);

        public double DraggerButtonHeight { 
            get {
                return (double)GetValue (DraggerButtonHeightProperty);
            }
            set {
                SetValue (DraggerButtonHeightProperty, value);
            }
        }

        public static readonly BindableProperty DraggerButtonWidthProperty = BindableProperty.Create (
                                                                                 "DraggerButtonWidth",
                                                                                 typeof(double),
                                                                                 typeof(SlideMenuView),
                                                                                 0.0);

        public double DraggerButtonWidth { 
            get {
                return (double)GetValue (DraggerButtonWidthProperty);
            }
            set {
                SetValue (DraggerButtonWidthProperty, value);
            }
        }

        public static readonly BindableProperty IsFullScreenProperty = BindableProperty.Create (
                                                                           "IsFullScreen", 
                                                                           typeof(bool), 
                                                                           typeof(SlideMenuView), 
                                                                           false);

        public bool IsFullScreen { 
            get {
                return (bool)GetValue (IsFullScreenProperty);
            }
            set {
                SetValue (IsFullScreenProperty, value);
            }
        }

        public static readonly BindableProperty AnimationDurationMillisecondProperty = BindableProperty.Create (
                                                                                           "AnimationDurationMillisecond", 
                                                                                           typeof(int), 
                                                                                           typeof(SlideMenuView), 
                                                                                           250);

        public int AnimationDurationMillisecond { 
            get {
                return (int)GetValue (AnimationDurationMillisecondProperty);
            }
            set {
                SetValue (AnimationDurationMillisecondProperty, value);
            }
        }

        public static readonly BindableProperty BackgroundViewColorProperty = BindableProperty.Create (
                                                                                  "BackgroundViewColor", 
                                                                                  typeof(Color), 
                                                                                  typeof(SlideMenuView), 
                                                                                  Color.Gray);

        public Color BackgroundViewColor { 
            get {
                return (Color)GetValue (BackgroundViewColorProperty);
            }
            set {
                SetValue (BackgroundViewColorProperty, value);
            }
        }

        internal Action HideEvent { get; set; }

        public void HideWithoutAnimations ()
        {
            if (HideEvent != null)
                HideEvent ();
        }

        public bool IsShown {
            get { 
                if (GetIsShown == null)
                    return false;
                else
                    return GetIsShown ();
            }
        }

        internal Func<bool> GetIsShown { get; set; }
    }
}


