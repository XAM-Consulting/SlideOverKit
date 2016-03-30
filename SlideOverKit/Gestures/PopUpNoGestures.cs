using System;

namespace SlideOverKit
{
    internal class PopUpNoGestures: GestureBase, IDragGesture, IDisposable
    {
        Rect _hidePosition;

        public PopUpNoGestures (SlideMenuView view, double density) : base (view, density)
        {
            CheckViewBound (view);
            UpdateLayoutSize (view);
            view.HideEvent = LayoutHideStatus;
        }

        void CheckViewBound (SlideMenuView view)
        {
            if (ScreenSizeHelper.ScreenHeight == 0 || ScreenSizeHelper.ScreenWidth == 0)
                throw new Exception ("Please set ScreenSizeHelper.ScreenHeight or ScreenSizeHelper.ScreenWidth");
            if (view.TopMargin <= 0)
                throw new Exception ("Please set SildeMenuView TopMargin");
            if (view.LeftMargin <= 0)
                throw new Exception ("Please set SildeMenuView LeftMargin");
        }

        public void UpdateLayoutSize (SlideMenuView view)
        {
            _left = view.LeftMargin * _density;
            _top = view.TopMargin * _density;
            _right = (ScreenSizeHelper.ScreenWidth - view.LeftMargin) * _density;
            if (view.IsFullScreen)
                _bottom = (ScreenSizeHelper.ScreenHeight - view.TopMargin) * _density;
            else
                _bottom = (ScreenSizeHelper.ScreenHeight - view.TopMargin - view.PageBottomOffset) * _density;

            switch (view.MenuOrientations) {
            case MenuOrientation.PopUpViewFromBottom:
                _hidePosition = new Rect () {
                    left = _left,
                    top = ScreenSizeHelper.ScreenHeight * _density,
                    right = _right,
                    bottom = ScreenSizeHelper.ScreenHeight * _density + _bottom - _top
                };
                break;
            case MenuOrientation.PopUpViewFromLeft:
                _hidePosition = new Rect (){ left = -(_right - _left), top = _top, right = 0, bottom = _bottom };
                break;
            case MenuOrientation.PopUpViewFromTop:
                _hidePosition = new Rect (){ left = _left, top = -(_bottom-_top), right = _right, bottom = 0 };
                break;
            case MenuOrientation.PopUpViewFromRight:
                _hidePosition = new Rect () {
                    left = ScreenSizeHelper.ScreenWidth * _density,
                    top = _top,
                    right = ScreenSizeHelper.ScreenWidth * _density + _right -_left,
                    bottom = _bottom
                };
                break;
            }
        }

        public void DragBegin (double x, double y)
        {            
        }

        public void DragMoving (double x, double y)
        {
        }


        public void DragFinished ()
        {   
        }

        public void LayoutShowStatus ()
        {
            if (RequestLayout != null) {
                RequestLayout (
                    _left,
                    _top,
                    _right,
                    _bottom,
                    _density);
            }
            if (NeedShowBackgroundView != null)
                NeedShowBackgroundView (true, 1);
        }

        public void LayoutHideStatus ()
        {
            if (RequestLayout != null) {
                RequestLayout (
                    _hidePosition.left,
                    _hidePosition.top,
                    _hidePosition.right,
                    _hidePosition.bottom,
                    _density);
            }
            if (NeedShowBackgroundView != null)
                NeedShowBackgroundView (false, 0);
        }

        public Rect GetShowPosition ()
        {           
            return new Rect () { 
                left = _left, 
                top = _top, 
                right = _right, 
                bottom = _bottom
            };
        }

        public Rect GetHidePosition ()
        {
            return _hidePosition;
        }

        public void Dispose ()
        {
            this.RequestLayout = null;
            this.NeedShowBackgroundView = null;
        }
    }
}

