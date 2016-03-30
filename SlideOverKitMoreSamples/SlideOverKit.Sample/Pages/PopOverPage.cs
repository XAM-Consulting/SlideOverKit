using System;
using Xamarin.Forms;

namespace SlideOverKit.MoreSample
{
    public class PopOverPage: MenuContainerPage
    {
        public PopOverPage ()
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
                }
            };

            this.SlideMenu = new PopOverView ();
        }
    }
}

