using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : TabbedPage
    {
        public HistoryPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            MessagingCenter.Subscribe<object, int>(this, "click", (arg, idx) =>
            {
                var chat = this.Children[idx] as ChatPage;
                var localUser = arg as User;
                //TODO
                //chat.LocalUser = localUser;
                CurrentPage = this.Children[idx];
            });
        }
    }
}