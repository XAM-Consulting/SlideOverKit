using System;

namespace SlideOverKit
{
    internal class HorizontalGestures: GestureBase, IDragGesture, IDisposable
    {
        double _leftMax, _leftMin, _rightMax, _rightMin = 0;
        bool _isLeftToRight = true;

        public HorizontalGestures (SlideMenuView view, double density) : base (view, density)
        {
            CheckViewBound (view);
            UpdateLayoutSize (view);
            view.HideEvent = LayoutHideStatus;
        }

        void CheckViewBound (SlideMenuView view)
        {
            if (ScreenSizeHelper.ScreenHeight == 0 || ScreenSizeHelper.ScreenWidth == 0)
                throw new Exception ("Please set ScreenSizeHelper.ScreenHeight or ScreenSizeHelper.ScreenWidth");
            if (view.WidthRequest <= 0)
                throw new Exception ("Please set SildeMenuView WidthRequest");            
        }

        public void UpdateLayoutSize (SlideMenuView view)
        {
            _leftMax = 0;
            _leftMin = -(view.WidthRequest - view.DraggerButtonWidth) * _density;
            _rightMax = view.WidthRequest * _density;
            _rightMin = view.DraggerButtonWidth * _density;
            if (view.MenuOrientations == MenuOrientation.RightToLeft) {
                _isLeftToRight = false;
                _leftMax = (ScreenSizeHelper.ScreenWidth - view.DraggerButtonWidth) * _density;
                _leftMin = (ScreenSizeHelper.ScreenWidth - view.WidthRequest) * _density;
                _rightMax = (ScreenSizeHelper.ScreenWidth + view.WidthRequest - view.DraggerButtonWidth) * _density;
                _rightMin = (ScreenSizeHelper.ScreenWidth) * _density;
            }
            if (!view.IsFullScreen) {
                _top = view.TopMargin * _density;
                _bottom = (view.TopMargin + view.HeightRequest) * _density;
            } else {
                _top = 0;
                _bottom = ScreenSizeHelper.ScreenHeight * _density;
            }
        }

        public void DragBegin (double x, double y)
        {
            _oldX = x;
            _willShown = true;
        }

        public void DragMoving (double x, double y)
        {
           
            double delta = x - _oldX;
            // Movement is too small on Android, so we treat it as click           
            if (delta > -2 && delta < 2) {
                return;
            }
            
            if (delta > 0)
                _willShown = !(true ^ _isLeftToRight);
            if (delta < 0)
                _willShown = !(false ^ _isLeftToRight);
            _left += delta;
            _right += delta;
            ChecklowerBound ();
            CheckUpperBound ();

            if (RequestLayout != null)
                RequestLayout (_left, _top, _right, _bottom, _density);
            if (NeedShowBackgroundView != null) {
                double backgoundViewAlpha = (_left - _leftMin) / (_leftMax - _leftMin);

                if (backgoundViewAlpha > 0 && backgoundViewAlpha < 1)
                    NeedShowBackgroundView (true, _isLeftToRight ? backgoundViewAlpha : 1 - backgoundViewAlpha);
                else {
                    if (_willShown)
                        NeedShowBackgroundView (true, 1);
                    else
                        NeedShowBackgroundView (false, 0);	
                }
            }
            _oldX = x;
        }

        void CheckUpperBound ()
        {
            _left = _left > _leftMax ? _leftMax : _left;
            _right = _right > _rightMax ? _rightMax : _right;
        }

        void ChecklowerBound ()
        {
            _left = _left < _leftMin ? _leftMin : _left;
            _right = _right < _rightMin ? _rightMin : _right;
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
            _willShown = true;
        }

        public Rect GetShowPosition ()
        {	
            _willShown = false;
            _left = _isLeftToRight ? _leftMax : _leftMin;
            _right = _isLeftToRight ? _rightMax : _rightMin;
            return new Rect () { 
                left = _left,
                top = _top,
                right = _right, 
                bottom = _bottom
            };
        }

        public Rect GetHidePosition ()
        {
            _willShown = true;
            _left = _isLeftToRight ? _leftMin : _leftMax;
            _right = _isLeftToRight ? _rightMin : _rightMax;
            return new Rect () {
                left = _left,
                top = _top,
                right = _right,
                bottom = _bottom
            };
        }

        public void Dispose ()
        {
            this.RequestLayout = null;
            this.NeedShowBackgroundView = null;
        }
    }
}

