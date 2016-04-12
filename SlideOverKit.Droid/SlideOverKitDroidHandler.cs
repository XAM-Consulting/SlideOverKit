using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using Android.Animation;

namespace SlideOverKit.Droid
{
    public class SlideOverKitDroidHandler
    {
        PageRenderer _pageRenderer;
        ISlideOverKitPageRendererDroid _menuKit;
        IMenuContainerPage _basePage;
        IDragGesture _dragGesture;
        IVisualElementRenderer _popMenuOverlayRenderer;
        global::Android.Widget.LinearLayout _backgroundOverlay;

        IPopupContainerPage _popupBasePage;
        IVisualElementRenderer _popupRenderer;
        string _currentPopup = null;

        public SlideOverKitDroidHandler ()
        {
        }


        public void Init (ISlideOverKitPageRendererDroid menuKit)
        {
            _menuKit = menuKit;
            _pageRenderer = menuKit as PageRenderer;

            _menuKit.OnElementChangedEvent = OnElementChanged;
            _menuKit.OnLayoutEvent = OnLayout;
            _menuKit.OnSizeChangedEvent = OnSizeChanged;
        }

        void OnElementChanged (ElementChangedEventArgs<Page> e)
        {
            _basePage = e.NewElement as IMenuContainerPage;
            _popupBasePage = e.NewElement as IPopupContainerPage;
            AddMenu ();
            AddPopup ();
        }

        void OnLayout (bool changed, int l, int t, int r, int b)
        {
            if (_popMenuOverlayRenderer != null) {
                _popMenuOverlayRenderer.UpdateLayout ();
                _pageRenderer.ViewGroup.BringChildToFront (_popMenuOverlayRenderer.ViewGroup);
            }
        }

        void AddMenu ()
        {
            if (_basePage == null)
                return;
            var menu = _basePage.SlideMenu;
            if (menu == null)
                return;

            _basePage.HideMenuAction = () => {
                if (_dragGesture == null)
                    return;
                var rect = _dragGesture.GetHidePosition ();
                _popMenuOverlayRenderer.ViewGroup.Animate ()
                    .X ((float)rect.left)
                    .Y ((float)rect.top)
                    .SetDuration (menu.AnimationDurationMillisecond)
                    .SetListener (new AnimatorListener (_dragGesture, false))
                    .Start ();             
            };

            _basePage.ShowMenuAction = () => {
                if (_dragGesture == null)
                    return;
                var rect = _dragGesture.GetShowPosition ();
                _popMenuOverlayRenderer.ViewGroup.Animate ()
                    .X ((float)rect.left)
                    .Y ((float)rect.top)
                    .SetDuration (menu.AnimationDurationMillisecond)
                    .SetListener (new AnimatorListener (_dragGesture, true))
                    .Start ();     
            };      

            if (_popMenuOverlayRenderer == null) {
                _popMenuOverlayRenderer = RendererFactory.GetRenderer (menu); 
                var metrics = _pageRenderer.Resources.DisplayMetrics;
                var rootView = _popMenuOverlayRenderer.ViewGroup;
                if (_popMenuOverlayRenderer is SlideMenuDroidRenderer) {
                    _dragGesture = (_popMenuOverlayRenderer as SlideMenuDroidRenderer).GragGesture;
                }
                if (_dragGesture == null)
                    return;
                var rect = _dragGesture.GetHidePosition ();

                menu.Layout (new Xamarin.Forms.Rectangle (
                    rect.left / metrics.Density, 
                    rect.top / metrics.Density, 
                    (rect.right - rect.left) / metrics.Density, 
                    (rect.bottom - rect.top) / metrics.Density));

                _popMenuOverlayRenderer.UpdateLayout ();
                _popMenuOverlayRenderer.ViewGroup.Visibility = ViewStates.Visible;
                rootView.Layout ((int)rect.left, (int)rect.top, (int)rect.right, (int)rect.bottom);
                _pageRenderer.ViewGroup.AddView (rootView);
                _pageRenderer.ViewGroup.BringChildToFront (rootView);

                _dragGesture.NeedShowBackgroundView = (open, alpha) => {
                    if (open)
                        ShowBackgroundOverlay (alpha);
                    else
                        HideBackgroundOverlay ();
                };
            }
        }

        void AddPopup ()
        {
            if (_popupBasePage == null)
                return;
            _popupBasePage.ShowPopupAction = (key) => {
                if (!string.IsNullOrEmpty (_currentPopup))
                    return;
                SlidePopupView popup = null;
                if (!_popupBasePage.PopupViews.ContainsKey (key)) {
                    if (string.IsNullOrEmpty (key) && _popupBasePage.PopupViews.Count == 1)
                        popup = _popupBasePage.PopupViews.Values.GetEnumerator ().Current;
                    if (popup == null)
                        return;
                }

                _currentPopup = key;
                popup = _popupBasePage.PopupViews [_currentPopup] as SlidePopupView;
                _popupRenderer = RendererFactory.GetRenderer (popup); 
                var rect = LayoutPopup ();

                _popupRenderer.ViewGroup.Visibility = ViewStates.Visible;
                _popupRenderer.ViewGroup.Layout ((int)rect.left, (int)rect.top, (int)rect.right, (int)rect.bottom);
                ShowBackgroundForPopup (popup.BackgroundViewColor.ToAndroid ());
                _pageRenderer.ViewGroup.AddView (_popupRenderer.ViewGroup);
                _pageRenderer.ViewGroup.BringChildToFront (_popupRenderer.ViewGroup);

                popup.HideMySelf = () => {
                    HideBackgroundForPopup ();
                };
            };

            _popupBasePage.HidePopupAction = () => {
                HideBackgroundForPopup ();
            };
        }

        Rect LayoutPopup ()
        {
            var popup = _popupBasePage.PopupViews [_currentPopup] as SlidePopupView;
            var metrics = _pageRenderer.Resources.DisplayMetrics;
            popup.CalucatePosition ();
            double x = popup.LeftMargin;
            double y = popup.TopMargin;
            double width = popup.WidthRequest <= 0 ? ScreenSizeHelper.ScreenWidth - popup.LeftMargin * 2 : popup.WidthRequest;
            double height = popup.HeightRequest <= 0 ? ScreenSizeHelper.ScreenHeight - popup.TopMargin * 2 : popup.HeightRequest;
            popup.Layout (new Xamarin.Forms.Rectangle (x, y, width, height));
            _popupRenderer.UpdateLayout ();
            return new Rect {
                left = x * metrics.Density, 
                top = y * metrics.Density, 
                right = (x + width) * metrics.Density, 
                bottom = (y + height) * metrics.Density
            };
        }

        void HideBackgroundOverlay ()
        {
            if (_backgroundOverlay != null) {
                _pageRenderer.RemoveView (_backgroundOverlay);
                _backgroundOverlay.Dispose ();
                _backgroundOverlay = null;
            }
        }

        void HideBackgroundForPopup ()
        {
            _currentPopup = null;
            if (_popupRenderer != null) {
                _pageRenderer.RemoveView (_popupRenderer.ViewGroup);
                _popupRenderer = null;
            }
            if (_backgroundOverlay != null) {
                _pageRenderer.RemoveView (_backgroundOverlay);
                _backgroundOverlay.Dispose ();
                _backgroundOverlay = null;
            }
        }

        void ShowBackgroundOverlay (double alpha)
        {
            if (_basePage == null)
                return;
            var menu = _basePage.SlideMenu;
            if (menu == null)
                return;

            double value = (double)(alpha * _basePage.SlideMenu.BackgroundViewColor.A);
            if (_backgroundOverlay != null) {
                var color = _basePage.SlideMenu.BackgroundViewColor.ToAndroid ();
                color.A = (Byte)(255 * value);
                _backgroundOverlay.SetBackgroundColor (color);
                return;
            }
            _backgroundOverlay = new global::Android.Widget.LinearLayout (Forms.Context);       
            _pageRenderer.ViewGroup.AddView (_backgroundOverlay);
            _backgroundOverlay.SetBackgroundColor (_basePage.SlideMenu.BackgroundViewColor.ToAndroid ());

            _backgroundOverlay.Touch += (object sender, Android.Views.View.TouchEventArgs e) => {
                _basePage.HideMenuAction ();
            };
            var metrics = _pageRenderer.Resources.DisplayMetrics;
            _backgroundOverlay.Layout (
                0, 
                0, 
                (int)(ScreenSizeHelper.ScreenWidth * metrics.Density), 
                (int)(ScreenSizeHelper.ScreenHeight * metrics.Density));

        }

        void ShowBackgroundForPopup (Android.Graphics.Color color)
        {
            if (_popupBasePage == null)
                return;
            if (_popupRenderer == null)
                return;
            _backgroundOverlay = new global::Android.Widget.LinearLayout (Forms.Context);       
            _pageRenderer.ViewGroup.AddView (_backgroundOverlay);
            _backgroundOverlay.SetBackgroundColor (color);

            _backgroundOverlay.Touch += (object sender, Android.Views.View.TouchEventArgs e) => {
                _popupBasePage.HidePopupAction ();
            };
            var metrics = _pageRenderer.Resources.DisplayMetrics;
            _backgroundOverlay.Layout (
                0, 
                0, 
                (int)(ScreenSizeHelper.ScreenWidth * metrics.Density), 
                (int)(ScreenSizeHelper.ScreenHeight * metrics.Density));

        }

        void OnSizeChanged (int w, int h, int oldw, int oldh)
        {
            if (_basePage == null)
                return;

            var metrics = _pageRenderer.Resources.DisplayMetrics;
            ScreenSizeHelper.ScreenWidth = w / metrics.Density;
            ScreenSizeHelper.ScreenHeight = h / metrics.Density;

            var menu = _basePage.SlideMenu;
            if (menu != null) {
                if (_dragGesture != null) {
                    _dragGesture.UpdateLayoutSize (menu);

                    var rect = _dragGesture.GetHidePosition ();
                    menu.Layout (new Xamarin.Forms.Rectangle (
                        rect.left / metrics.Density, 
                        rect.top / metrics.Density, 
                        (rect.right - rect.left) / metrics.Density, 
                        (rect.bottom - rect.top) / metrics.Density));
                    if (_popMenuOverlayRenderer != null)
                        _popMenuOverlayRenderer.UpdateLayout ();
                    _dragGesture.LayoutHideStatus ();
                    return;
                }
            }

            if (!string.IsNullOrEmpty (_currentPopup)) {
                LayoutPopup ();
            }

            if (_backgroundOverlay != null)
                _backgroundOverlay.Layout (
                    0, 
                    0, 
                    (int)(ScreenSizeHelper.ScreenWidth * metrics.Density), 
                    (int)(ScreenSizeHelper.ScreenHeight * metrics.Density));
        }
    }

    class  AnimatorListener  :Java.Lang.Object, Android.Animation.Animator.IAnimatorListener
    {
        IDragGesture _dragGesture;
        bool _isShow;

        public AnimatorListener (IDragGesture dragGesture, bool isShow)
        {
            _dragGesture = dragGesture;
            _isShow = isShow;
        }

        public void OnAnimationCancel (Animator animation)
        {

        }

        public void OnAnimationEnd (Animator animation)
        {
            if (_isShow)
                _dragGesture.NeedShowBackgroundView.Invoke (true, 1);
            else
                _dragGesture.NeedShowBackgroundView.Invoke (false, 0);
        }

        public void OnAnimationRepeat (Animator animation)
        {

        }

        public void OnAnimationStart (Animator animation)
        {

        }


    }
}

