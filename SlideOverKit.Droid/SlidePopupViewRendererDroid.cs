using System;
using Xamarin.Forms;
using SlideOverKit;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof(SlidePopupView), typeof(SlideOverKit.Droid.SlidePopupViewRendererDroid))]
namespace SlideOverKit.Droid
{
    public class SlidePopupViewRendererDroid: VisualElementRenderer<SlidePopupView>
    {
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

