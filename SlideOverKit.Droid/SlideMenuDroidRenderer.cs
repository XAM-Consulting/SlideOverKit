using System;
using Xamarin.Forms;
using SlideOverKit.Droid;
using Xamarin.Forms.Platform.Android;
using SlideOverKit;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer (typeof(SlideMenuView), typeof(SlideMenuDroidRenderer))]
namespace SlideOverKit.Droid
{
    public class SlideMenuDroidRenderer : ViewRenderer <SlideMenuView, Android.Views.View>
    {
        IDragGesture _dragGesture;

        internal IDragGesture GragGesture { get { return _dragGesture; } }

        public SlideMenuDroidRenderer():base()
        {

        }

        public SlideMenuDroidRenderer (Context context):base(context)
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<SlideMenuView> e)
        {
            base.OnElementChanged (e);
            InitDragGesture ();
        }

        void InitDragGesture ()
        {
            var menu = Element as SlideMenuView;
            if (menu == null)
                return;
            if (ScreenSizeHelper.ScreenHeight == 0 && ScreenSizeHelper.ScreenWidth == 0) {               
                ScreenSizeHelper.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
                ScreenSizeHelper.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;
            }            
            _dragGesture = DragGestureFactory.GetGestureByView (menu, this.Resources.DisplayMetrics.Density);
            _dragGesture.RequestLayout = (l, t, r, b, desity) => {
                this.SetX ((float)l);
                this.SetY ((float)t);
            };           
        }
         
        public override bool OnTouchEvent (MotionEvent e)
        {
            if (_dragGesture == null)
                return false;
            MotionEventActions action = e.Action & MotionEventActions.Mask;
            if (action == MotionEventActions.Down)
                _dragGesture.DragBegin (e.RawX, e.RawY);   
            if (action == MotionEventActions.Move)
                _dragGesture.DragMoving (e.RawX, e.RawY);
            if (action == MotionEventActions.Up)
                _dragGesture.DragFinished ();
            return true;
        }

    }
}

