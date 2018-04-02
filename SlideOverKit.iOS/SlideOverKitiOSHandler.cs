using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;
using System.Linq;

namespace SlideOverKit.iOS
{
    public class SlideOverKitiOSHandler
    {
        PageRenderer _pageRenderer;
        ISlideOverKitPageRendereriOS _menuKit;

        IMenuContainerPage _basePage;
        IVisualElementRenderer _menuOverlayRenderer;
        UIPanGestureRecognizer _panGesture;
        IDragGesture _dragGesture;

        IPopupContainerPage _popupBasePage;
        UIView _popupNativeView;
        string _currentPopup = null;

        public SlideOverKitiOSHandler ()
        {
        }

        public void Init (ISlideOverKitPageRendereriOS menuKit)
        {
            _menuKit = menuKit;
            _pageRenderer = menuKit as PageRenderer;

            _menuKit.ViewDidAppearEvent = ViewDidAppear;
            _menuKit.OnElementChangedEvent = OnElementChanged;
            _menuKit.ViewDidLayoutSubviewsEvent = ViewDidLayoutSubviews;
            _menuKit.ViewDidDisappearEvent = ViewDidDisappear;
            _menuKit.ViewWillTransitionToSizeEvent = ViewWillTransitionToSize;
        }

        bool CheckPageAndMenu ()
        {
            if (_basePage != null && _basePage.SlideMenu != null)
                return true;
            else
                return false;
        }

        bool CheckPageAndPopup ()
        {
            if (_popupBasePage != null && _popupBasePage.PopupViews != null && _popupBasePage.PopupViews.Count > 0)
                return true;
            else
                return false;
        }

        UIView _backgroundOverlay;

        void HideBackgroundOverlay ()
        {
            if (_backgroundOverlay != null) {
                _backgroundOverlay.RemoveFromSuperview ();
                _backgroundOverlay.Dispose ();
                _backgroundOverlay = null;
            }
            _menuOverlayRenderer?.NativeView?.EndEditing (true);
        }

        void HideBackgroundForPopup ()
        {
            _currentPopup = null;
            if (_popupNativeView != null) {
                _popupNativeView.RemoveFromSuperview ();
                _popupNativeView = null;
            }
            if (_backgroundOverlay != null) {
                _backgroundOverlay.RemoveFromSuperview ();
                _backgroundOverlay.Dispose ();
                _backgroundOverlay = null;
            }
        }

        void ShowBackgroundOverlay (double alpha)
        {
            if (!CheckPageAndMenu ())
                return;
            nfloat value = (nfloat)(alpha * _basePage.SlideMenu.BackgroundViewColor.A);
            if (_backgroundOverlay != null) {
                _backgroundOverlay.BackgroundColor = _basePage.SlideMenu.BackgroundViewColor.ToUIColor ().ColorWithAlpha (value);
                return;
            }
            _backgroundOverlay = new UIView ();
            _backgroundOverlay.BackgroundColor = _basePage.SlideMenu.BackgroundViewColor.ToUIColor ().ColorWithAlpha (value);

            _backgroundOverlay.AddGestureRecognizer (new UITapGestureRecognizer (() => {
                this._basePage.HideMenuAction ();
            }));

            if (_basePage.SlideMenu.IsFullScreen) {
                _backgroundOverlay.Frame = new CGRect (UIApplication.SharedApplication.KeyWindow.Frame.Location, UIApplication.SharedApplication.KeyWindow.Frame.Size);
                UIApplication.SharedApplication.KeyWindow.InsertSubviewBelow (_backgroundOverlay, _menuOverlayRenderer.NativeView);
            } else {
                _backgroundOverlay.Frame = new CGRect (_pageRenderer.View.Frame.Location, _pageRenderer.View.Frame.Size);
                _pageRenderer.View.InsertSubviewBelow (_backgroundOverlay, _menuOverlayRenderer.NativeView);
            }
        }

        void ShowBackgroundForPopup (UIColor color)
        {
            if (!CheckPageAndPopup ())
                return;

            if (_backgroundOverlay != null) {
                _backgroundOverlay.BackgroundColor = color;
                return;
            }
            _backgroundOverlay = new UIView ();
            _backgroundOverlay.BackgroundColor = color;

            _backgroundOverlay.AddGestureRecognizer (new UITapGestureRecognizer (() => {
                this._popupBasePage.HidePopupAction ();
            }));

            _backgroundOverlay.Frame = new CGRect (_pageRenderer.View.Frame.Location, _pageRenderer.View.Frame.Size);
            _pageRenderer.View.AddSubview (_popupNativeView);
            _pageRenderer.View.InsertSubviewBelow (_backgroundOverlay, _popupNativeView);
        }

        void LayoutMenu ()
        {
            if (!CheckPageAndMenu ())
                return;

            // areadly add gesture
            if (_dragGesture != null)
                return;

            var menu = _basePage.SlideMenu;

            _dragGesture = DragGestureFactory.GetGestureByView (menu);
            _dragGesture.RequestLayout = (l, t, r, b, density) => {
                _menuOverlayRenderer.NativeView.Frame = new CGRect (l, t, (r - l), (b - t));
                _menuOverlayRenderer.NativeView.SetNeedsLayout ();
            };
            _dragGesture.NeedShowBackgroundView = (open, alpha) => {
                UIView.CommitAnimations ();
                if (open)
                    ShowBackgroundOverlay (alpha);
                else
                    HideBackgroundOverlay ();
            };

            _basePage.HideMenuAction = () => {
                UIView.BeginAnimations ("OpenAnimation");
                UIView.SetAnimationDuration (((double)menu.AnimationDurationMillisecond) / 1000);
                _dragGesture.LayoutHideStatus ();

            };

            _basePage.ShowMenuAction = () => {
                UIView.BeginAnimations ("OpenAnimation");
                UIView.SetAnimationDuration (((double)menu.AnimationDurationMillisecond) / 1000);
                _dragGesture.LayoutShowStatus ();
            };

            if (_menuOverlayRenderer == null) {
                _menuOverlayRenderer = Platform.CreateRenderer (menu);
                Platform.SetRenderer (menu, _menuOverlayRenderer);

                _panGesture = new UIPanGestureRecognizer (() => {
                    var p0 = _panGesture.LocationInView (_pageRenderer.View);
                    if (_panGesture.State == UIGestureRecognizerState.Began) {
                        _dragGesture.DragBegin (p0.X, p0.Y);

                    } else if (_panGesture.State == UIGestureRecognizerState.Changed
                               && _panGesture.NumberOfTouches == 1) {
                        _dragGesture.DragMoving (p0.X, p0.Y);

                    } else if (_panGesture.State == UIGestureRecognizerState.Ended) {
                        _dragGesture.DragFinished ();
                    }
                });
                _menuOverlayRenderer.NativeView.AddGestureRecognizer (_panGesture);
            }

            var rect = _dragGesture.GetHidePosition ();
            menu.Layout (new Rectangle (
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top)));
            _menuOverlayRenderer.NativeView.Hidden = !menu.IsVisible;
            _menuOverlayRenderer.NativeView.Frame = new CGRect (
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top));
            _menuOverlayRenderer.NativeView.SetNeedsLayout ();

        }

        void LayoutPopup ()
        {
            if (!CheckPageAndPopup ())
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
                var renderer = Platform.CreateRenderer (popup);
                Platform.SetRenderer (popup, renderer);
                _popupNativeView = renderer.NativeView;

                CGRect pos = GetPopupPositionAndLayout ();
                if (pos.IsEmpty)
                    return;

                _popupNativeView.Hidden = false;

                if (_popupNativeView != null) {
                    ShowBackgroundForPopup (popup.BackgroundViewColor.ToUIColor ());
                    popup.IsShown = true;
                }
                popup.HideMySelf = () => {
                    HideBackgroundForPopup ();
                    popup.IsShown = false;
                };
            };

            _popupBasePage.HidePopupAction = () => {
                HideBackgroundForPopup ();
                var popup = _popupBasePage.PopupViews.Values.Where (o => o.IsShown).FirstOrDefault ();
                if (popup != null)
                    popup.IsShown = false;
            };
        }

        CGRect GetPopupPositionAndLayout ()
        {
            if (string.IsNullOrEmpty (_currentPopup))
                return CGRect.Empty;
            var popup = _popupBasePage.PopupViews [_currentPopup] as SlidePopupView;

            // This should layout with LeftMargin, TopMargin, WidthRequest and HeightRequest
            CGRect pos;
            popup.CalucatePosition ();
            nfloat y = (nfloat)popup.TopMargin;
            nfloat x = (nfloat)popup.LeftMargin;
            nfloat width = (nfloat)(popup.WidthRequest <= 0 ? ScreenSizeHelper.ScreenWidth - popup.LeftMargin * 2 : popup.WidthRequest);
            nfloat height = (nfloat)(popup.HeightRequest <= 0 ? ScreenSizeHelper.ScreenHeight - popup.TopMargin * 2 : popup.HeightRequest);

            pos = new CGRect (x, y, width, height);
            popup.Layout (pos.ToRectangle ());

            _popupNativeView.Frame = pos;
            _popupNativeView.SetNeedsLayout ();

            return pos;

        }

        public void OnElementChanged (VisualElementChangedEventArgs e)
        {
            _basePage = e.NewElement as IMenuContainerPage;
            _popupBasePage = e.NewElement as IPopupContainerPage;

            ScreenSizeHelper.ScreenHeight = UIScreen.MainScreen.Bounds.Height;
            ScreenSizeHelper.ScreenWidth = UIScreen.MainScreen.Bounds.Width;

            LayoutMenu ();
            LayoutPopup ();
        }

        public void ViewDidLayoutSubviews ()
        {
            GetPopupPositionAndLayout ();
        }

        public void ViewDidAppear (bool animated)
        {
            if (!CheckPageAndMenu ())
                return;
            if (_basePage.SlideMenu.IsFullScreen)
                UIApplication.SharedApplication.KeyWindow.AddSubview (_menuOverlayRenderer.NativeView);
            else
                _pageRenderer.View.AddSubview (_menuOverlayRenderer.NativeView);
        }

        public void ViewDidDisappear (bool animated)
        {
            if (_menuOverlayRenderer != null)
                _menuOverlayRenderer.NativeView.RemoveFromSuperview ();
            HideBackgroundOverlay ();
            HideBackgroundForPopup ();
        }


        public void ViewWillTransitionToSize (CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            var menu = _basePage.SlideMenu;

            // this is used for rotation 
            double bigValue = UIScreen.MainScreen.Bounds.Height > UIScreen.MainScreen.Bounds.Width ? UIScreen.MainScreen.Bounds.Height : UIScreen.MainScreen.Bounds.Width;
            double smallValue = UIScreen.MainScreen.Bounds.Height < UIScreen.MainScreen.Bounds.Width ? UIScreen.MainScreen.Bounds.Height : UIScreen.MainScreen.Bounds.Width;
            if (toSize.Width < toSize.Height) {
                ScreenSizeHelper.ScreenHeight = bigValue;
                // this is used for mutiltasking
                ScreenSizeHelper.ScreenWidth = toSize.Width < smallValue ? toSize.Width : smallValue;
            } else {
                ScreenSizeHelper.ScreenHeight = smallValue;
                ScreenSizeHelper.ScreenWidth = toSize.Width < bigValue ? toSize.Width : bigValue;
            }

            if (!string.IsNullOrEmpty (_currentPopup)) {
                GetPopupPositionAndLayout ();

                // Layout background
                _backgroundOverlay.Frame = new CGRect (0, 0, ScreenSizeHelper.ScreenWidth, ScreenSizeHelper.ScreenHeight);
            }


            if (_dragGesture == null)
                return;

            _dragGesture.UpdateLayoutSize (menu);
            var rect = _dragGesture.GetHidePosition ();
            menu.Layout (new Xamarin.Forms.Rectangle (
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top)));
            _dragGesture.LayoutHideStatus ();

        }
    }
}

