using System;

using Xamarin.Forms;
using System.Collections.Generic;

namespace SlideOverKit
{
    public class MenuContainerPage : ContentPage, IMenuContainerPage, IPopupContainerPage
    {
        public MenuContainerPage ()
        {
            PopupViews = new Dictionary<string, SlidePopupView> ();
        }

        SlideMenuView slideMenu;
        public SlideMenuView SlideMenu {
            get {
                return slideMenu;
            }
            set {
                if (slideMenu != null)
                    slideMenu.Parent = null;
                slideMenu = value;
                if (slideMenu != null)
                    slideMenu.Parent = this;
            }
        }

        public Action ShowMenuAction { get; set; }

        public Action HideMenuAction { get; set; }

        public Dictionary<string, SlidePopupView> PopupViews { get; set; }

        public Action<string>  ShowPopupAction { get; set; }

        public Action HidePopupAction { get; set; }

        /// <summary>
        /// This is a property that people using mvvm frameworks can bind to, as an alternative to explicitely calling
        /// ShowMenu and HideMenu methods directly. 
        /// </summary>
        public bool IsMenuVisible
        {
            get { return (bool)GetValue(IsMenuVisibleProperty); }
            set { SetValue(IsMenuVisibleProperty, value); }
        }

        /// <summary>
        /// The backing bindable property for IsMenuVisible
        /// </summary>
        public static readonly BindableProperty IsMenuVisibleProperty =
            BindableProperty.Create("IsMenuVisible", typeof(bool), typeof(MenuContainerPage), false, BindingMode.TwoWay, null,
                (bindable, oldValue, newValue) =>
                {
                    //One property changed we hide or show the menu. 
                    if (oldValue != newValue)
                    {
                        var thisView = (MenuContainerPage)bindable;
                        if ((bool)newValue) thisView.ShowMenu();
                        else thisView.HideMenu();
                    }
                },
                (bindable, oldValue, newValue) =>
                {
                    // Property is changing
                },
                (bindable, value) =>
                {
                    // Coerce the property
                    // Here we would override what the passed in value is
                    // And return something else - if we wanted to
                    return value;
                },
                 (bindable) =>
                 {
                     // This is the default creator
                     return false;
                 });

        public void ShowMenu ()
        {
            if (ShowMenuAction != null)
                ShowMenuAction ();
        }

        public void HideMenu ()
        {
            if (HideMenuAction != null)
                HideMenuAction ();
        }

        public void ShowPopup (string name)
        {
            if (ShowPopupAction != null)
                ShowPopupAction (name);
        }

        public void HidePopup ()
        {
            if (HidePopupAction != null)
                HidePopupAction ();
        }
    }
}


