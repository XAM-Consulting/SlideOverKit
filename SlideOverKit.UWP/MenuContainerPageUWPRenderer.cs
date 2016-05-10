using SlideOverKit;
using SlideOverKit.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.Foundation;

[assembly: ExportRenderer(typeof(MenuContainerPage), typeof(MenuContainerPageUWPRenderer))]
namespace SlideOverKit.UWP
{
    public class MenuContainerPageUWPRenderer : PageRenderer, ISlideOverKitPageRendererUWP
    {
        public Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }
        SlideOverKitUWPHandler _handler;

        public MenuContainerPageUWPRenderer()
        {
            _handler = new SlideOverKitUWPHandler();
            _handler.Init(this);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            if (OnElementChangedEvent != null)
                OnElementChangedEvent(e);
        }

        protected override void Dispose(bool disposing)
        {
            _handler.Dispose();
            base.Dispose(disposing);
            _handler = null;
        }

    }
}
