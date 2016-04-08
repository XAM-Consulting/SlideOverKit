using System;
using Xamarin.Forms;

namespace SlideOverKit
{
    public class SlidePopupView : Frame
    {
        public static readonly BindableProperty LeftMarginProperty = BindableProperty.Create (nameof (TopMargin), typeof(double), typeof(SlidePopupView), default(double));

        public double LeftMargin {
            get { return (double)GetValue (LeftMarginProperty); }
            set { SetValue (LeftMarginProperty, value); }
        }

        public static readonly BindableProperty TopMarginProperty = BindableProperty.Create (nameof (TopMargin), typeof(double), typeof(SlidePopupView), default(double));

        public double TopMargin {
            get { return (double)GetValue (TopMarginProperty); }
            set { SetValue (TopMarginProperty, value); }
        }

        public static readonly BindableProperty BackgroundViewColorProperty = BindableProperty.Create (nameof (BackgroundViewColor), typeof(Color), typeof(SlidePopupView), Color.Gray);

        public Color BackgroundViewColor { 
            get { return (Color)GetValue (BackgroundViewColorProperty); }
            set { SetValue (BackgroundViewColorProperty, value); }
        }

        //TODO add this these later
//        public static readonly BindableProperty HaveTriangleProperty = BindableProperty.Create (nameof (HaveTriangle), typeof(bool), typeof(SlidePopupView), default(bool));
//
//        public bool HaveTriangle {
//            get { return (bool)GetValue (HaveTriangleProperty); }
//            set { SetValue (HaveTriangleProperty, value); }
//        }
//
//        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create (nameof (CornerRadius), typeof(int), typeof(SlidePopupView), default(int));
//
//        public int CornerRadius {
//            get { return (int)GetValue (CornerRadiusProperty); }
//            set { SetValue (CornerRadiusProperty, value); }
//        }

        public VisualElement TargetControl { get; set; }

        public Action HideMySelf { get; internal set; }

        public void Hide ()
        {            
            if (HideMySelf != null)
                HideMySelf ();
        }

        internal void CalucatePosition ()
        {
            if (!(LeftMargin == 0 && TopMargin == 0))
                return;
            if (TargetControl == null)
                return;
            LeftMargin += TargetControl.X + TargetControl.Width / 2 - this.WidthRequest / 2;
            TopMargin += TargetControl.Y + TargetControl.Height;
            var parent = TargetControl.Parent;
            while (!(parent == null || parent is IPopupContainerPage)) {
                LeftMargin += (parent as VisualElement).X;
                TopMargin += (parent as VisualElement).Y;
                parent = parent.Parent;
            }
        }

        public SlidePopupView() :base()
        {
            this.Padding = new Thickness (0);
            this.HasShadow = false;
        }
    }
}

