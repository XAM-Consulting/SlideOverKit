using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;

namespace SlideOverKit.iOS
{
    public interface ISlideOverKitPageRendereriOS
    {
        Action<bool> ViewDidAppearEvent { get; set; }

        Action<VisualElementChangedEventArgs> OnElementChangedEvent { get; set; }

        Action<CGSize, IUIViewControllerTransitionCoordinator> ViewWillTransitionToSizeEvent { get; set; }

        Action ViewDidLayoutSubviewsEvent { get; set; }

        Action<bool> ViewDidDisappearEvent { get; set; }
    }
}

