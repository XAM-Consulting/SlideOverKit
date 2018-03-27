using System;
using Xamarin.Forms;

namespace SlideOverKit
{
    public class SlidePopupView : Frame
    {
        public static readonly BindableProperty LeftMarginProperty = BindableProperty.Create (nameof (TopMargin), typeof (double), typeof (SlidePopupView), default (double));

        public double LeftMargin {
            get { return (double)GetValue (LeftMarginProperty); }
            set { SetValue (LeftMarginProperty, value); }
        }

        public static readonly BindableProperty TopMarginProperty = BindableProperty.Create (nameof (TopMargin), typeof (double), typeof (SlidePopupView), default (double));

        public double TopMargin {
            get { return (double)GetValue (TopMarginProperty); }
            set { SetValue (TopMarginProperty, value); }
        }

        public static readonly BindableProperty BackgroundViewColorProperty = BindableProperty.Create (nameof (BackgroundViewColor), typeof (Color), typeof (SlidePopupView), Color.Gray);

        public Color BackgroundViewColor {
            get { return (Color)GetValue (BackgroundViewColorProperty); }
            set { SetValue (BackgroundViewColorProperty, value); }
        }

        public static readonly BindableProperty AdjustXProperty = BindableProperty.Create (nameof (AdjustX), typeof (double), typeof (SlidePopupView), default (double));

        public double AdjustX {
            get { return (double)GetValue (AdjustXProperty); }
            set { SetValue (AdjustXProperty, value); }
        }

        public static readonly BindableProperty AdjustYProperty = BindableProperty.Create (nameof (AdjustY), typeof (double), typeof (SlidePopupView), default (double));

        public double AdjustY {
            get { return (double)GetValue (AdjustYProperty); }
            set { SetValue (AdjustYProperty, value); }
        }

        public VisualElement TargetControl { get; set; }

        public Action HideMySelf { get; set; }

        public void Hide ()
        {
            if (HideMySelf != null)
                HideMySelf ();
        }

        public void CalucatePosition (Point? point = null)
        {
            // In this case, popup layout need Left and top margin, 
            // we need not to calucate the position, no matter the sreen orientation 
            if (TargetControl == null)
                return;

            Point newPos;
            if (!point.HasValue) {
                newPos = new Point {
                    X = TargetControl.X,
                    Y = TargetControl.Y
                };
            } else {
                newPos = point.Value;
            }

            // In this case, we need to calucate the position every time based on the Target control
            // before we do that we need to set Left and Top Margin as 0
            LeftMargin = 0;
            TopMargin = 0;

            if (this.HorizontalOptions.Alignment == LayoutAlignment.Start)
                LeftMargin = newPos.X;
            else if (this.HorizontalOptions.Alignment == LayoutAlignment.End)
                LeftMargin += newPos.X + TargetControl.Width / 2;
            else
                LeftMargin += newPos.X + TargetControl.Width / 2 - this.WidthRequest / 2;

            if (this.VerticalOptions.Alignment == LayoutAlignment.Start)
                TopMargin += newPos.Y;
            else if (this.VerticalOptions.Alignment == LayoutAlignment.End)
                TopMargin += newPos.Y + TargetControl.Height;
            else
                TopMargin += newPos.Y + (TargetControl.Height / 2);


            // point is used in Android, cause it can get the parent control position from API
            // Therefore no need to calculate position like this
            // Get position in iOS
            if (!point.HasValue) {
                var parent = TargetControl.Parent;
                while (!(parent == null || parent is IPopupContainerPage)) {
                    if (parent is ScrollView) {
                        LeftMargin -= (parent as ScrollView).ScrollX;
                        TopMargin -= (parent as ScrollView).ScrollY;
                    }
                    LeftMargin += (parent as VisualElement).X;
                    TopMargin += (parent as VisualElement).Y;
                    parent = parent.Parent;
                }
            }

            LeftMargin += AdjustX;
            TopMargin += AdjustY;
        }

        public SlidePopupView () : base ()
        {
            this.Padding = new Thickness (0);
            this.HasShadow = false;
        }

        public bool IsShown { get; set; }
    }
}

