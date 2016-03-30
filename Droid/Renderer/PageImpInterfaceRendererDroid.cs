using System;
using Xamarin.Forms;
using SlideOverKit.Sample;
using SlideOverKit.Sample.Droid;
using Xamarin.Forms.Platform.Android;
using SlideOverKit.Droid;

[assembly: ExportRenderer (typeof(PageImpInterface), typeof(PageImpInterfaceRendererDroid))]
namespace SlideOverKit.Sample.Droid
{
    public class PageImpInterfaceRendererDroid : PageRenderer, ISlideOverKitPageRendererDroid
    {
        public Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }

        public Action<bool, int,int,int,int> OnLayoutEvent { get; set; }

        public Action<int,int,int,int> OnSizeChangedEvent { get; set; }

        public PageImpInterfaceRendererDroid ()
        {
            new SlideOverKitDroidHandler ().Init (this);
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

