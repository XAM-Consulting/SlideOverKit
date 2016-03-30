using System;

namespace SlideOverKit
{
    internal class DragGestureFactory
    {
        internal static IDragGesture GetGestureByView (SlideMenuView view, double density = 1.0f)
        {
            switch (view.MenuOrientations) {
            case MenuOrientation.TopToBottom:
                return new VerticalGesture (view, density);
            case MenuOrientation.BottomToTop:
                return new VerticalGesture (view, density);
            case MenuOrientation.LeftToRight:
                return new HorizontalGestures (view, density);
            case MenuOrientation.RightToLeft:
                return new HorizontalGestures (view, density);
            case MenuOrientation.PopUpViewFromBottom:
            case MenuOrientation.PopUpViewFromLeft:
            case MenuOrientation.PopUpViewFromRight:
            case MenuOrientation.PopUpViewFromTop:
                return new PopUpNoGestures (view, density);
            default:
                return new VerticalGesture (view, density);
            }
        }
    }

    internal class GestureBase
    {
        protected double _oldX, _oldY, _left, _right, _top, _bottom = 0;
        protected double _density = 1;
        protected bool _willShown = false;

        internal GestureBase (SlideMenuView view, double density)
        {
            _density = density;
        }



        public Action<double, double, double, double, double> RequestLayout {
			get;
			set;
		}

        public Action<bool, double> NeedShowBackgroundView { 
			get; 
			set; 
		}
    }
}

