using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]

namespace SlideOverKit.MoreSample
{
    public class App : Application
    {
        public App ()
        {
            MainPage = new NavigationPage (new SlideOverKit.MoreSample.MainPage ()){ Title = "Main Page" };
            MessagingCenter.Subscribe<SlideOverKit.MoreSample.QuickInnerMenuPage> (this, "GoBackToMainPage", (m) => {
                Device.BeginInvokeOnMainThread (() => {                    
                    MainPage = new NavigationPage (new SlideOverKit.MoreSample.MainPage ()){ Title = "Main Page" };
                });
            });
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

