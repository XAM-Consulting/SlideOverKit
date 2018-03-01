using System;
using Xamarin.Forms;
using SlideOverKit;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer (typeof(SlidePopupView), typeof(SlideOverKit.Droid.SlidePopupViewRendererDroid))]
namespace SlideOverKit.Droid
{
    public class SlidePopupViewRendererDroid: VisualElementRenderer<SlidePopupView>
    {
        public SlidePopupViewRendererDroid(Context context):base(context)
        {
            
        }
        protected override void OnElementChanged (ElementChangedEventArgs<SlidePopupView> e)
        {
            base.OnElementChanged (e);
            if (ScreenSizeHelper.ScreenHeight == 0 && ScreenSizeHelper.ScreenWidth == 0) {               
                ScreenSizeHelper.ScreenWidth = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density;
                ScreenSizeHelper.ScreenHeight = Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density;
            }   
        }

    }
}

