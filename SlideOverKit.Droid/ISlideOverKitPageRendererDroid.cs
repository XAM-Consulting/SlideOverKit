using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace SlideOverKit.Droid
{
    public interface ISlideOverKitPageRendererDroid
    {
        Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }

        Action<bool, int,int,int,int> OnLayoutEvent { get; set; }

        Action<int,int,int,int> OnSizeChangedEvent { get; set; }
    }
}

