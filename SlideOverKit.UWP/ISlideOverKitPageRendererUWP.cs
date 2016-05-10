using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace SlideOverKit.UWP
{
    public interface ISlideOverKitPageRendererUWP
    {
        Action<ElementChangedEventArgs<Page>> OnElementChangedEvent { get; set; }      
    }
}
