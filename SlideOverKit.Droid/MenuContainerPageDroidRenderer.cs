using System;
using Xamarin.Forms;
using SlideOverKit;
using Xamarin.Forms.Platform.Android;
using SlideOverKit.Droid;
using Android.Views;
using Android.Content;

[assembly: ExportRenderer (typeof(MenuContainerPage), typeof(MenuContainerPageDroidRenderer))]
namespace SlideOverKit.Droid
{
    public class MenuContainerPageDroidRenderer  : PageRenderer, ISlideOverKitPageRendererDroid
    {
        public Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }

        public Action<bool, int,int,int,int> OnLayoutEvent { get; set; }

        public Action<int,int,int,int> OnSizeChangedEvent { get; set; }

        public MenuContainerPageDroidRenderer (Context context):base(context)
        {
            new SlideOverKitDroidHandler ().Init (this, context);
        }

        protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged (e);
            if (OnElementChangedEvent != null)
                OnElementChangedEvent (e);
        }

        protected override void OnLayout (bool changed, int l, int t, int r, int b)
        {
            base.OnLayout (changed, l, t, r, b);
            if (OnLayoutEvent != null)
                OnLayoutEvent (changed, l, t, r, b);
        }

        protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged (w, h, oldw, oldh);
            if (OnSizeChangedEvent != null)
                OnSizeChangedEvent (w, h, oldw, oldh);
        }
    }
}

