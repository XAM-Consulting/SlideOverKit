using System;

using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class RightSideDetailPage : MenuContainerPage
    {
        public RightSideDetailPage ()
        {
            Content = new StackLayout { 
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Children = {
                    new Button{
                        Text ="Show Menu",
                        Command = new Command(()=>{
                            this.ShowMenu();
                        })
                    },
                    new Button{
                        Text ="Hide Menu",
                        Command = new Command(()=>{
                            this.HideMenu();
                        })
                    },
                }
            };

            this.SlideMenu = new RightSideMasterPage ();
        }
    }
}


