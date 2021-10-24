using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Helpers;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            chatListViewModel.InitSubscription();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            chatListViewModel.Chats.SafeClear();
            chatListViewModel.Subscription.Dispose();
        }
    }
}