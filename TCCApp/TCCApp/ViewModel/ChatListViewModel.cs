using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ChatListViewModel))]
namespace TCCApp.ViewModel
{
    public class ChatListViewModel : BaseViewModel
    {
        public ObservableCollection<ChatList> Chats { get; set; }

        private double deleteButtonOpacity;

        public double DeleteButtonOpacity
        {
            get { return deleteButtonOpacity; }
            set => Set(ref deleteButtonOpacity, value);
        }

        private SelectionMode chatSelectionMode;

        public SelectionMode ChatSelectionMode
        {
            get { return chatSelectionMode; }
            set => Set(ref chatSelectionMode, value);
        }

        ChatViewModel chatViewModel;
        public ChatListViewModel()
        {
            ChatSelectionMode = SelectionMode.Single;
            DeleteButtonOpacity = 0.3;
            chatViewModel = DependencyService.Get<ChatViewModel>();
            Chats = new ObservableCollection<ChatList>
            {
                new ChatList
                {
                    Author = "Lucia",
                    GroupKey = "Conversa1"
                },
                new ChatList
                {
                    Author = "Junior",
                    GroupKey = "Conversa2"
                },
                new ChatList
                {
                    Author = "Ronaldo",
                    GroupKey = "Conversa3"
                },
            };
        }

        //Todo comando temporario
        public ICommand SelectMultiple => new Command(async() =>
        {
            //Chatlist em modo de delete
            DeleteButtonOpacity = 1;
            ChatSelectionMode = SelectionMode.Multiple;
            await Application.Current.MainPage.DisplayAlert("Modo Delete Ativado","Selecione as conversas que deseja deletar","ok");
        });

        //Todo - Adicionar ref ao banco
        public ICommand DeleteChat => new Command((s) =>
        {
            var collectionView = s as CollectionView;

            var chats = collectionView.SelectedItems;

            if (chats != null)
            {
                foreach (ChatList chat in chats)
                {
                    Chats.Remove(chat);
                }
            }

            //Volta a chatlist para a configuração padrão
            DeleteButtonOpacity = 0.3;
            ChatSelectionMode = SelectionMode.Single;
        });

        public ICommand GoToChat => new Command(async sender =>
        {
            CollectionView view = sender as CollectionView;

            if (view.SelectedItem != null && ChatSelectionMode == SelectionMode.Single)
            {
                var selected = view.SelectedItem as Notification;

                chatViewModel.Author = App.user.Nome;

                chatViewModel.GroupKey = selected.GroupKey;

                await Application.Current.MainPage.Navigation.PushAsync(new ChatPage());

                view.SelectedItem = null;
            }
        });
    }
}
