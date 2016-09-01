using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace SlideOverKit.UWP
{
    public class SlideOverKitUWPHandler : IDisposable
    {
        PageRenderer _pageRenderer;
        ISlideOverKitPageRendererUWP _menuKit;
        IMenuContainerPage _basePage;
        IDragGesture _dragGesture;
        LayoutRenderer _popMenuOverlayRenderer;

        IPopupContainerPage _popupBasePage;
        FrameRenderer _popupRenderer;
        string _currentPopup = null;
        Windows.UI.Xaml.Controls.Canvas _mainCanvas;
        uint _pointID = uint.MaxValue;

        public SlideOverKitUWPHandler()
        {
        }


        public void Init(ISlideOverKitPageRendererUWP menuKit)
        {
            _menuKit = menuKit;
            _pageRenderer = menuKit as PageRenderer;
            _menuKit.OnElementChangedEvent = OnElementChanged;
        }

        void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
        {
            _basePage = e.NewElement as IMenuContainerPage;
            e.NewElement.Disappearing += NewElement_Disappearing;
            e.NewElement.Appearing += NewElement_Appearing;
            _popupBasePage = e.NewElement as IPopupContainerPage;
            FindRootCanvas(Windows.UI.Xaml.Window.Current.Content);

            ScreenSizeHelper.ScreenHeight = ApplicationView.GetForCurrentView().VisibleBounds.Height;
            ScreenSizeHelper.ScreenWidth = ApplicationView.GetForCurrentView().VisibleBounds.Width;

            LayoutMenu();
            LayoutPopup();
        }

        private void NewElement_Appearing(object sender, EventArgs e)
        {
            if (_mainCanvas == null)
            {
                FindRootCanvas(Windows.UI.Xaml.Window.Current.Content);
            }
            if (_mainCanvas != null)
            {
                AddControlToCanvas();
            }
        }

        private void NewElement_Disappearing(object sender, EventArgs e)
        {
            if (_mainCanvas != null)
            {
                if (_popMenuOverlayRenderer != null)
                {
                    _mainCanvas.Children.Remove(_popMenuOverlayRenderer);
                }
                if (_backgroundOverlay != null)
                {
                    _mainCanvas.Children.Remove(_backgroundOverlay);
                    _backgroundOverlay = null;
                }
                _mainCanvas.SizeChanged -= mainCanvas_SizeChanged;
            }
        }

        void FindRootCanvas(UIElement root)
        {
            if (root == null)
                return;
            if (root is Canvas)
            {
                _mainCanvas = root as Canvas;
                return;
            }
            if (root is ContentControl)
            {
                FindRootCanvas((root as ContentControl).Content as UIElement);
                return;
            }
            if (root is UserControl)
            {
                FindRootCanvas((root as UserControl).Content as UIElement);
                return;
            }
        }

        bool CheckPageAndMenu()
        {
            if (_basePage != null
                && _basePage.SlideMenu != null)
                return true;
            else
                return false;
        }

        bool CheckPageAndPopup()
        {
            if (_popupBasePage != null
                && _popupBasePage.PopupViews != null
                && _popupBasePage.PopupViews.Count > 0)
                return true;
            else
                return false;
        }

        Windows.UI.Xaml.Shapes.Rectangle _backgroundOverlay;

        void HideBackgroundOverlay()
        {
            if (_backgroundOverlay != null)
            {
                _mainCanvas.Children.Remove(_backgroundOverlay);
                _backgroundOverlay = null;
            }
        }

        void HideBackgroundForPopup()
        {
            _currentPopup = null;
            if (_popupRenderer != null)
            {
                _mainCanvas.Children.Remove(_popupRenderer);
                _popupRenderer = null;
            }
            if (_backgroundOverlay != null)
            {
                _mainCanvas.Children.Remove(_backgroundOverlay);
                _backgroundOverlay = null;
            }
        }

        Windows.UI.Color ConvertToWindowsColor(Color xamarinColor)
        {
            return new Windows.UI.Color
            {
                A = Convert.ToByte(xamarinColor.A * 255),
                R = Convert.ToByte(xamarinColor.R * 255),
                G = Convert.ToByte(xamarinColor.G * 255),
                B = Convert.ToByte(xamarinColor.B * 255)
            };
        }

        void ShowBackgroundOverlay(double alpha)
        {
            if (!CheckPageAndMenu())
                return;
            var color = ConvertToWindowsColor(_basePage.SlideMenu.BackgroundViewColor);
            if (_backgroundOverlay != null)
            {
                color.A = Convert.ToByte(alpha * color.A);
                _backgroundOverlay.Fill = new SolidColorBrush(color);
                return;
            }
            _backgroundOverlay = new Windows.UI.Xaml.Shapes.Rectangle();
            _backgroundOverlay.Fill = new SolidColorBrush(color);
            _backgroundOverlay.Width = ScreenSizeHelper.ScreenWidth;
            _backgroundOverlay.Height = ScreenSizeHelper.ScreenHeight;

            _backgroundOverlay.Tapped += _backgroundOverlay_Tapped;

            _mainCanvas.Children.Add(_backgroundOverlay);
            Canvas.SetZIndex(_backgroundOverlay, 253);
        }

        private void _backgroundOverlay_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (_basePage != null)
                _basePage.HideMenuAction?.Invoke();
            if (_popupBasePage != null)
                _popupBasePage.HidePopupAction?.Invoke();
        }

        private void ShowBackgroundForPopup(Color color)
        {
            if (!CheckPageAndPopup())
                return;
            var uwpcolor = ConvertToWindowsColor(color);
            if (_backgroundOverlay != null)
            {
                _backgroundOverlay.Fill = new SolidColorBrush(uwpcolor);
                return;
            }
            _backgroundOverlay = new Windows.UI.Xaml.Shapes.Rectangle();
            _backgroundOverlay.Fill = new SolidColorBrush(uwpcolor);
            _backgroundOverlay.Width = ScreenSizeHelper.ScreenWidth;
            _backgroundOverlay.Height = ScreenSizeHelper.ScreenHeight;

            _backgroundOverlay.Tapped += _backgroundOverlay_Tapped;
            _mainCanvas.Children.Add(_backgroundOverlay);
            Canvas.SetZIndex(_backgroundOverlay, 253);
        }

        void LayoutMenu()
        {
            if (!CheckPageAndMenu())
                return;
            var menu = _basePage.SlideMenu;
            _dragGesture = DragGestureFactory.GetGestureByView(menu);
            _dragGesture.RequestLayout = (l, t, r, b, density) =>
            {
                menu.Layout(new Xamarin.Forms.Rectangle(
                l,
                t,
                (r - l),
                (b - t)));
                Canvas.SetLeft(_popMenuOverlayRenderer, l);
                Canvas.SetTop(_popMenuOverlayRenderer, t);
                Canvas.SetZIndex(_popMenuOverlayRenderer, 254);
                _popMenuOverlayRenderer.UpdateLayout();
                _mainCanvas.UpdateLayout();
            };
            _dragGesture.NeedShowBackgroundView = (open, alpha) =>
            {
                if (open)
                    ShowBackgroundOverlay(alpha);
                else
                    HideBackgroundOverlay();
            };

            _basePage.HideMenuAction = () =>
            {
                if (_dragGesture == null)
                    return;
                _dragGesture.LayoutHideStatus();
                // DoubleAnimation wasn't supported by UWP?
                //DoubleAnimation dal = new DoubleAnimation();
                //dal.From = Canvas.GetLeft(_popMenuOverlayRenderer);
                //dal.To = rectp.left;
                //dal.Duration = new Duration(TimeSpan.FromMilliseconds(menu.AnimationDurationMillisecond));
                //DoubleAnimation dat = new DoubleAnimation();
                //dat.From = Canvas.GetTop(_popMenuOverlayRenderer);
                //dat.To = rectp.top;
                //dat.Duration = new Duration(TimeSpan.FromMilliseconds(menu.AnimationDurationMillisecond));
                //(_popMenuOverlayRenderer as UIElement).BeginAnimation(Canvas.LeftProperty, dal);

            };

            _basePage.ShowMenuAction = () =>
            {
                if (_dragGesture == null)
                    return;
                _dragGesture.LayoutShowStatus();
            };
            if (_popMenuOverlayRenderer == null)
            {
                _popMenuOverlayRenderer = Platform.CreateRenderer(menu) as LayoutRenderer;
                _popMenuOverlayRenderer.PointerPressed += menuOverlayRenderer_PointerPressed;
                _popMenuOverlayRenderer.PointerMoved += menuOverlayRenderer_PointerMoved;
                _popMenuOverlayRenderer.PointerReleased += menuOverlayRenderer_PointerReleased;
                _popMenuOverlayRenderer.PointerExited += menuOverlayRenderer_PointerReleased;
            }
            var rect = _dragGesture.GetHidePosition();
            menu.Layout(new Xamarin.Forms.Rectangle(
                rect.left,
                rect.top,
                (rect.right - rect.left),
                (rect.bottom - rect.top)));
            Canvas.SetLeft(_popMenuOverlayRenderer, rect.left);
            Canvas.SetTop(_popMenuOverlayRenderer, rect.top);
            _popMenuOverlayRenderer.Visibility = menu.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            _popMenuOverlayRenderer.UpdateLayout();
        }

        private void LayoutPopup()
        {
            if (!CheckPageAndPopup())
                return;
            _popupBasePage.ShowPopupAction = (key) =>
            {
                if (!string.IsNullOrEmpty(_currentPopup))
                    return;
                SlidePopupView popup = null;
                if (!_popupBasePage.PopupViews.ContainsKey(key))
                {
                    if (string.IsNullOrEmpty(key) && _popupBasePage.PopupViews.Count == 1)
                        popup = _popupBasePage.PopupViews.Values.GetEnumerator().Current;
                    if (popup == null)
                        return;
                }

                _currentPopup = key;
                popup = _popupBasePage.PopupViews[_currentPopup] as SlidePopupView;
                var renderer = popup.GetOrCreateRenderer();
                _popupRenderer = renderer as FrameRenderer;

                if (_popupRenderer != null)
                {
                    if (!PopupLayout())
                        return;
                    _mainCanvas.Children.Add(_popupRenderer);
                    Canvas.SetZIndex(_popupRenderer, 254);
                    _popupRenderer.Visibility = Visibility.Visible;

                    ShowBackgroundForPopup(popup.BackgroundViewColor);
                    popup.IsShown = true;
                }

                popup.HideMySelf = () =>
                {
                    HideBackgroundForPopup();
                    popup.IsShown = false;
                };
            };

            _popupBasePage.HidePopupAction = () =>
            {
                HideBackgroundForPopup();
                var popup = _popupBasePage.PopupViews.Values.Where(o => o.IsShown).FirstOrDefault();
                if (popup != null)
                    popup.IsShown = false;
            };
        }

        bool PopupLayout()
        {
            if (string.IsNullOrEmpty(_currentPopup))
                return false;
            var popup = _popupBasePage.PopupViews[_currentPopup] as SlidePopupView;

            Point? position = null;
            if (popup.TargetControl!=null)
            {
                var nativeElement = popup.TargetControl.GetOrCreateRenderer() as Panel;
                var ttv = nativeElement.TransformToVisual(Window.Current.Content);
                var wpos = ttv.TransformPoint(new Windows.Foundation.Point(0, 0));
                position = new Point(wpos.X, wpos.Y);
            }

            popup.CalucatePosition(position);
            double y = popup.TopMargin;
            double x = popup.LeftMargin;
            double width = (popup.WidthRequest <= 0 ? ScreenSizeHelper.ScreenWidth - popup.LeftMargin * 2 : popup.WidthRequest);
            double height = (popup.HeightRequest <= 0 ? ScreenSizeHelper.ScreenHeight - popup.TopMargin * 2 : popup.HeightRequest);

            popup.Layout(new Rectangle(x, y, width, height));
            if (_popupRenderer != null)
            {
                Canvas.SetLeft(_popupRenderer, x);
                Canvas.SetTop(_popupRenderer, y);
            }
            return true;
        }

        void AddControlToCanvas()
        {
            if (_popMenuOverlayRenderer != null)
                _mainCanvas.Children.Add(_popMenuOverlayRenderer);
            _mainCanvas.UpdateLayout();
            _mainCanvas.SizeChanged += mainCanvas_SizeChanged;
            ScreenSizeHelper.ScreenWidth = _mainCanvas.ActualWidth;
            ScreenSizeHelper.ScreenHeight = _mainCanvas.ActualHeight;
            UpdateMenuWithNewSize();
        }

        private void menuOverlayRenderer_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(_mainCanvas);
            if (point.PointerId != _pointID)
                return;
            _dragGesture.DragFinished();
            _pointID = uint.MaxValue;
            e.Handled = true;
        }

        private void menuOverlayRenderer_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(_mainCanvas);
            if (point.PointerId != _pointID)
                return;
            _dragGesture.DragMoving(point.Position.X, point.Position.Y);
            e.Handled = true;
        }

        private void menuOverlayRenderer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(_mainCanvas);
            _dragGesture.DragBegin(point.Position.X, point.Position.Y);
            _pointID = point.PointerId;
            e.Handled = true;
        }

        private void mainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScreenSizeHelper.ScreenWidth = e.NewSize.Width;
            ScreenSizeHelper.ScreenHeight = e.NewSize.Height;
            UpdateMenuWithNewSize();

            if (!string.IsNullOrEmpty(_currentPopup))
            {
                PopupLayout();
                _backgroundOverlay.Width = ScreenSizeHelper.ScreenWidth;
                _backgroundOverlay.Height = ScreenSizeHelper.ScreenHeight;

            }
        }

        void UpdateMenuWithNewSize()
        {
            var menu = _basePage.SlideMenu;
            if (menu != null)
            {
                if (menu.IsFullScreen)
                {
                    if (menu.MenuOrientations == MenuOrientation.BottomToTop
                        || menu.MenuOrientations == MenuOrientation.TopToBottom)
                        menu.WidthRequest = ScreenSizeHelper.ScreenWidth;
                    if (menu.MenuOrientations == MenuOrientation.LeftToRight
                        || menu.MenuOrientations == MenuOrientation.RightToLeft)
                        menu.HeightRequest = ScreenSizeHelper.ScreenHeight;
                }
                if (_dragGesture != null)
                {
                    _dragGesture.UpdateLayoutSize(menu);
                    _dragGesture.LayoutHideStatus();
                }
            }
        }

        public void Dispose()
        {
            if (_mainCanvas != null)
            {
                if (_popMenuOverlayRenderer != null)
                {
                    _popMenuOverlayRenderer.PointerPressed -= menuOverlayRenderer_PointerPressed;
                    _popMenuOverlayRenderer.PointerMoved -= menuOverlayRenderer_PointerMoved;
                    _popMenuOverlayRenderer.PointerReleased -= menuOverlayRenderer_PointerReleased;
                    _popMenuOverlayRenderer.PointerExited -= menuOverlayRenderer_PointerReleased;
                    _mainCanvas.Children.Remove(_popMenuOverlayRenderer);
                }
                if (_backgroundOverlay != null)
                {
                    _backgroundOverlay.Tapped -= _backgroundOverlay_Tapped;
                    _mainCanvas.Children.Remove(_backgroundOverlay);
                }
                if (_popupRenderer != null)
                {
                    _mainCanvas.Children.Remove(_popupRenderer);
                }
                _mainCanvas.SizeChanged -= mainCanvas_SizeChanged;
            }
            _mainCanvas = null;
            _popMenuOverlayRenderer = null;
            _backgroundOverlay = null;
            _popupRenderer = null;
        }
    }
}
