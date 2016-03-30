SlideOverKit

SlideOverKit is cross-platform components, it based on Xamarin Form and can work on iOS, Android and Windows Universal App.

It quick, light and easy to use. It's based on Xamarin Form, so you can add XAML codes in SlidMenuView. SlidMenuView can appear on all direction, top, bottom, left and right. You can set SlidMenuView size and position. Also you can show or hide SlidMenuView by codes.

Setup:

First install SlideOverKit for all project, Xamarin Form PCL, Android, iOS and UWP.

In iOS, you have to call 
SlideOverKitiOS.Init (UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
After global::Xamarin.Forms.Forms.Init ();

No other settings for Android and UWP.
For UWP project, we use Preview version, please follow this document to configure UWP with Xamarin Forms
http://forums.xamarin.com/discussion/54401/xamarin-forms-for-uwp-preview-now-available

Usage:
Please put your menu UI in SlideMenuView class, you should inherit it and add your controls, also you can use it in XAML.
There are few property you need to set, before use it
	SlideMenuView.MenuOrientations : it used for set menu appear direction, the default value is from Left to Right.
	Positions:
	You must set SlideMenuView.WidthRequest and SlideMenuView.HeightRequest, otherwise it will not show the menu.
	SlideMenuView.IsFullScreen : If you set IsFullScreen as true, it means it will occupy full screen width or height. Therefore it will ignore WidthRequest if your menu set direction as TopToBottom or BottomToTop, and it will ignore HeightRequest if your menu direction is LeftToRight or RightToLeft.

	If your menu is not full screen, you can set LeftMargin when your menu direction is TopToBottom or BottomToTop, or you can set TopMargin when your menu direction is LeftToRight or RightToLeft.

	DraggerButtonHeight/DraggerButtonWidth is used for setting the drag offset in your menu. You can put the drag image into your menu, and keep the Height/Width is the same with DraggerButtonHeight/DraggerButtonWidth. In this case DraggerButtonHeight is used for menu direction as TopToBottom or BottomToTop, and DraggerButtonWidth used for TopToBottom or BottomToTop.

	PageBottomOffset it only can be used in Android and the menu direction is BottomToTop. As SlideMenuView cannot layout full screen in Android, so the top position of menu is incorrectly, you need to adjust PageBottomOffset to keep the top menu in the correct position.

	AnimationDurationMillisecond is used for set the speed of Show or Hide menu animation. 

The page which contains SlideMenuView must inherit from MenuContainerPage or IMenuContainerPage. If your page don't need inherit from other base page, you can inherit from MenuContainerPage and do nothing.

If your page need to inherit from other pages, you must implement IMenuContainerPage interface, and you need set the renderer pages for each platform. You can follow the sample codes to do this.



