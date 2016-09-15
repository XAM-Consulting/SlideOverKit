using System;

namespace SlideOverKit
{
    internal class VerticalGesture : GestureBase, IDragGesture, IDisposable
    {
        double _topMax, _topMin, _bottomMax, _bottomMin = 0;
        bool _isToptoBottom = true;

        public VerticalGesture (SlideMenuView view, double density) : base (view, density)
        {
            CheckViewBound (view);
            UpdateLayoutSize (view);
            view.HideEvent = LayoutHideStatus;
        }

        void CheckViewBound (SlideMenuView view)
        {
            if (ScreenSizeHelper.ScreenHeight == 0 || ScreenSizeHelper.ScreenWidth == 0)
                throw new Exception ("Please set ScreenSizeHelper.ScreenHeight or ScreenSizeHelper.ScreenWidth");
            if (view.HeightRequest <= 0)
                throw new Exception ("Please set SildeMenuView HeightRequest");
        }

        public void UpdateLayoutSize (SlideMenuView view)
        {
            _topMax = 0;
            _topMin = -(view.HeightRequest - view.DraggerButtonHeight) * _density;
            _bottomMax = view.HeightRequest * _density;
            _bottomMin = view.DraggerButtonHeight * _density;
            if (view.MenuOrientations == MenuOrientation.BottomToTop) {
                _isToptoBottom = false;
                _topMax = (ScreenSizeHelper.ScreenHeight - view.DraggerButtonHeight) * _density;
                _topMin = (ScreenSizeHelper.ScreenHeight - view.HeightRequest) * _density;
                _bottomMax = (ScreenSizeHelper.ScreenHeight + view.HeightRequest - view.DraggerButtonHeight) * _density;
                _bottomMin = ScreenSizeHelper.ScreenHeight * _density;
            }
            if (!view.IsFullScreen) {
                _left = view.LeftMargin * _density;
                _right = (view.LeftMargin + view.WidthRequest) * _density;
            } else {
                _left = 0;
                _right = ScreenSizeHelper.ScreenWidth * _density;
            }
        }

        public void DragBegin (double x, double y)
        {
            _oldY = y;
            _willShown = true;
        }

        public void DragMoving (double x, double y)
        {
            double delta = y - _oldY;
            // Movement is too small on Android, so we treat it as click           
            if (delta > -2 && delta < 2)
                return;
            
            if (delta > 0)
                _willShown = !(true ^ _isToptoBottom);
            if (delta < 0)
                _willShown = !(false ^ _isToptoBottom);
            _top += delta;
            _bottom += delta;
            CheckUpperBound ();
            ChecklowerBound ();

            if (RequestLayout != null)
                RequestLayout (_left, _top, _right, _bottom, _density);

            if (NeedShowBackgroundView != null) {
                double backgoundViewAlpha = (_top - _topMin) / (_topMax - _topMin);

                if (backgoundViewAlpha > 0 && backgoundViewAlpha < 1)
                    NeedShowBackgroundView (true, _isToptoBottom ? backgoundViewAlpha : 1 - backgoundViewAlpha);
                else {
                    if (_willShown)
                        NeedShowBackgroundView (true, 1);
                    else
                        NeedShowBackgroundView (false, 0);	
                }
            }
            _oldY = y;
        }

        void CheckUpperBound ()
        {
            _top = _top > _topMax ? _topMax : _top;
            _bottom = _bottom > _bottomMax ? _bottomMax : _bottom;
        }

        void ChecklowerBound ()
        {
            _top = _top < _topMin ? _topMin : _top;
            _bottom = _bottom < _bottomMin ? _bottomMin : _bottom;
        }

        public void DragFinished ()
        {
            if (_willShown)
                LayoutShowStatus ();
            else
                LayoutHideStatus ();	
        }

        public void LayoutShowStatus ()
        {
            if (RequestLayout != null) {
                GetShowPosition ();
                RequestLayout (_left, _top, _right, _bottom, _density);
            }
            if (NeedShowBackgroundView != null)
                NeedShowBackgroundView (true, 1);
        }

        public void LayoutHideStatus ()
        {
            if (RequestLayout != null) {
                GetHidePosition ();
                RequestLayout (_left, _top, _right, _bottom, _density);
            }
            if (NeedShowBackgroundView != null)
                NeedShowBackgroundView (false, 0);
        }

        public Rect GetShowPosition ()
        {
            _willShown = false;
            _top = _isToptoBottom ? _topMax : _topMin;
            _bottom = _isToptoBottom ? _bottomMax : _bottomMin;
            return new Rect () { 
                left = _left, 
                top = _isToptoBottom ? _topMax : _topMin, 
                right = _right, 
                bottom = _isToptoBottom ? _bottomMax : _bottomMin
            };
        }

        public Rect GetHidePosition ()
        {
            _willShown = true;
            _top = _isToptoBottom ? _topMin : _topMax;
            _bottom = _isToptoBottom ? _bottomMin : _bottomMax;
            return new Rect () { 
                left = _left, 
                top = _isToptoBottom ? _topMin : _topMax,
                right = _right, 
                bottom = _isToptoBottom ? _bottomMin : _bottomMax
            };
        }

        public void Dispose ()
        {
            this.RequestLayout = null;
            this.NeedShowBackgroundView = null;
        }
    }
}

