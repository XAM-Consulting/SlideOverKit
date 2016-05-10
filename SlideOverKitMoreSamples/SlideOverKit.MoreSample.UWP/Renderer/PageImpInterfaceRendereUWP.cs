using SlideOverKit.MoreSample;
using SlideOverKit.MoreSample.UWP.Renderer;
using SlideOverKit.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SlideDownMenuPage), typeof(PageImpInterfaceRendereUWP))]
namespace SlideOverKit.MoreSample.UWP.Renderer
{
    public class PageImpInterfaceRendereUWP : PageRenderer, ISlideOverKitPageRendererUWP
    {
        public Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }
        SlideOverKitUWPHandler _handler;

        public PageImpInterfaceRendereUWP()
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
