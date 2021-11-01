using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ClickedUserViewModel))]
namespace TCCApp.ViewModel
{
    public class ClickedUserViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; private set; }
        public User ClickedUser { get; set; }

        private ImageSource userImage;
        public ImageSource UserImage
        {
            get { return userImage; }
            set => Set(ref userImage, value);
        }

        private string sobre;
        public string Sobre
        {
            get { return sobre; }
            set => Set(ref sobre, value);
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set => Set(ref nome, value);
        }

        public ClickedUserViewModel()
        {
            Items = new ObservableCollection<Item>();
        }
        public async void InitClickedView()
        {
            UserImage = ImageSource.FromStream(() => new MemoryStream(ClickedUser.Buffer));
            Nome = ClickedUser.Nome;
            Sobre = ClickedUser.Sobre;
            var items = await DatabaseService.GetItems(ClickedUser.Key);

            foreach (Item item in items)
            {
                Items.Add(item);
            }
        }
        public ICommand CreateChat => new Command(async() =>
        {
            //Todo Tente trocar as chaves de posicao dps
            string chatKey = $"{App.user.Key}{ClickedUser.Key}";

            //Verifico se a conversa existe
            var chat = await DatabaseService.GetChat(chatKey);
            if (chat == null)
            {
                //Crio uma nova conversa
                DatabaseService.NewChat(chatKey);

                //Envio uma notificacao para o clickedUser
                DatabaseService.SendNotification(ClickedUser.Key, new Notification
                {
                    Author = App.user.Nome,
                    GroupKey = chatKey,
                    ByteImage = App.user.Buffer
                });
                //Adiciono uma nova conversa na minha lista de conversa
                DatabaseService.AddToChatList(App.user.Key, new ChatList
                {
                    Author = ClickedUser.Nome,
                    ByteImage = ClickedUser.Buffer,
                    GroupKey = chatKey
                });
                //Vou para o chat
                await Application.Current.MainPage.Navigation.PushAsync(new ChatPage(App.user.Nome, chatKey));
            }
            else
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ChatPage(App.user.Nome, chatKey));
            }
        });
    }
}
