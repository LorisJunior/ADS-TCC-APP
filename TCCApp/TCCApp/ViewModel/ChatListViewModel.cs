using Firebase.Database.Query;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ChatListViewModel))]
namespace TCCApp.ViewModel
{
    public class ChatListViewModel : BaseViewModel
    {
        public ObservableCollection<ChatList> Chats { get; set; }

        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public IDisposable Subscription { get; set; }

        private double deleteButtonOpacity;

        public double DeleteButtonOpacity
        {
            get { return deleteButtonOpacity; }
            set => Set(ref deleteButtonOpacity, value);
        }

        public bool IsDeleting { get; set; }

        private SelectionMode chatSelectionMode;

        public SelectionMode ChatSelectionMode
        {
            get { return chatSelectionMode; }
            set => Set(ref chatSelectionMode, value);
        }

        public ChatListViewModel()
        {
            IsDeleting = false;
            ChatSelectionMode = SelectionMode.Single;
            DeleteButtonOpacity = 0.3;
            Chats = new ObservableCollection<ChatList>();
        }
        
        public async void InitSubscription()
        {
            await semaphoreSlim.WaitAsync();

            var observable = DatabaseService
                .firebase
                .Child("UserChatList")
                .Child(App.user.Key).AsObservable<ChatList>();

            Subscription = observable
            .Where(f => !string.IsNullOrEmpty(f.Key))
            .Subscribe(f =>
            {
                if (!IsDeleting)
                {
                    var chat = new ChatList
                    {
                        chatListKey = f.Key,
                        Author = f.Object.Author,
                        MyImage = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer)),
                        Image = ImageSource.FromStream(() => new MemoryStream(f.Object.ByteImage)),
                        GroupKey = f.Object.GroupKey
                    };
                    Chats.Add(chat);
                }
            });

            semaphoreSlim.Release();
        }
        public ICommand SelectMultiple => new Command(async() =>
        {
            if (Chats.Count > 0)
            {
                //Chatlist em modo de delete
                DeleteButtonOpacity = 1;
                ChatSelectionMode = SelectionMode.Multiple;
                await Application.Current.MainPage.DisplayAlert("Modo delete ativado", "Selecione as conversas que deseja deletar", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Você não tem conversas", "Você não tem conversas para deletar", "ok");
            }
        });
        //Todo - Adicionar ref ao banco
        public ICommand DeleteChat => new Command(async(s) =>
        {
            await semaphoreSlim.WaitAsync();

            IsDeleting = true;

            var collectionView = s as CollectionView;

            var chats = collectionView.SelectedItems;

            if (chats != null)
            {
                foreach (ChatList chat in chats)
                {
                    //Remove da observablecollection
                    Chats.Remove(chat);
                    //Remove da minha lista de conversas
                    await DatabaseService.DeleteChatList(chat.chatListKey);
                    //Remove a conversa da tabela Chat
                    await DatabaseService.DeleteChat(chat.GroupKey);
                }
            }

            //Volta a chatlist para a configuração padrão
            DeleteButtonOpacity = 0.3;
            ChatSelectionMode = SelectionMode.Single;
            IsDeleting = false;
            semaphoreSlim.Release();
        });
        public ICommand GoToChat => new Command(async sender =>
        {
            CollectionView view = sender as CollectionView;

            if (view.SelectedItem != null && ChatSelectionMode == SelectionMode.Single)
            {
                var selected = view.SelectedItem as Notification;

                await Application.Current.MainPage.Navigation.PushAsync(new ChatPage(App.user.Nome, selected.GroupKey));

                view.SelectedItem = null;
            }
        });
    }
}
