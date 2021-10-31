using TCCApp.Helpers;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        ChatViewModel chatViewModel;
        public ChatPage(string author, string groupKey)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            chatViewModel = DependencyService.Get<ChatViewModel>();
            chatViewModel.GroupKey = groupKey;
            chatViewModel.Author = author;

            BindingContext = chatViewModel;

            chatViewModel.Messages.CollectionChanged += Messages_CollectionChanged;
        }

        private void Messages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (chatViewModel.Messages.Count > 0)
            {
                MessageList.ScrollTo(chatViewModel.Messages.Count - 1);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            chatViewModel.InitMessages();

            chatViewModel.InitSubscription();

            //Ao iniciar vai até a ultima mensagem recebida
            if (chatViewModel.Messages.Count > 0)
            {
                MessageList.ScrollTo(chatViewModel.Messages.Count - 1);
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            //Limpa a lista de mensagens
            chatViewModel.Messages.SafeClear();

            //Deixa de observar as atualizações desta conversa
            chatViewModel.Subscription.Dispose();
        }
    }
}