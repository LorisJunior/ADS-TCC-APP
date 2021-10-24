using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListPage : ContentPage
    {
        ChatListViewModel chatListViewModel;
        public ChatListPage()
        {
            InitializeComponent();
            chatListViewModel = DependencyService.Get<ChatListViewModel>();
            BindingContext = chatListViewModel;
        }
    }
}