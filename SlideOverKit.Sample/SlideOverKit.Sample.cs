using System;

using Xamarin.Forms;

namespace SlideOverKit.Sample
{
    public class App : Application
    {
        public App ()
        {           
            SlideOverKit.Licenses.Key = "OMJA1pFSWjPEO1ujfhVHe2kG49yGyOlgLWwzbrHmNfddNjU1/GBg9HzhQmhCgWCMpADl9Uv68Kc4lM9YZXNNXHSuA0bD578LD23ACUPvAZOgyMzoodacv80lAJ2y0J256NM7/yeZkZew8sV16LS+CBm8YurLJysCkoWF/rlAulc=";

            // The root page of your application
            MainPage = new NavigationPage(
                new MainPage(new MenuView(MenuOrientation.RightToLeft)
                    {IsFullScreen = true, AnimationDurationMillisecond = 1000}))
            {Title = "MainPage", BarBackgroundColor = Color.FromHex("#2D81C0")}; 

            // Sample for the page cannot inherit from MenuContainerPage
//            MainPage = new NavigationPage(
//                new PageImpInterface(new MenuView(MenuOrientation.PopUpViewFromBottom)
//                    {LeftMargin = 30,
//                        TopMargin =30}))
//            {Title = "MainPage", BarBackgroundColor = Color.FromHex("#2D81C0")}; 

            // If you use menu from bottom to top and it is used in TabbedPage or Naviation page
            // you need to set PageBottomOffset in MenuView
            // For example tabbedPage
//            var tabPage = new TabbedPage ();
//            tabPage.Children.Add (new MainPage (new MenuView (MenuOrientation.BottomToTop) { 
//                PageBottomOffset = Device.OnPlatform<double> (0, 60, 42),
//                IsFullScreen = true
//            }) { Title = "MainPage" });
//            MainPage = tabPage;

            // Sample for Navigation page
//            MainPage = new NavigationPage (new MainPage (new MenuView (MenuOrientation.BottomToTop){
//                PageBottomOffset = Device.OnPlatform<double> (0, 60, 42),
//                IsFullScreen = true
//            })){ Title = "MainPage" };
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}

