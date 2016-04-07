using System;
using Xamarin.Forms;
using SlideOverKit;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer (typeof(SlidePopupView), typeof(SlideOverKit.iOS.SlidePopupViewRendereriOS))]
namespace SlideOverKit.iOS
{
    public class SlidePopupViewRendereriOS : VisualElementRenderer<SlidePopupView>
    {
      
    }
}

