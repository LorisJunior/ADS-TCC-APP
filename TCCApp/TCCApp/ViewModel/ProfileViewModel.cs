using Firebase.Database.Query;
using Plugin.Media;
using Plugin.Media.Abstractions;
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

[assembly:Dependency(typeof(ProfileViewModel))]
namespace TCCApp.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        public ObservableCollection<Notification> Notifications { get; private set; }
        public IDisposable Subscription { get; set; }
        //Serve para evitar que quando eu delete algo, o subscription perceba a alteração e carregue a ui de novo
        public bool IsDeleting { get; set; }

        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private Color displayButtonColor;
        public Color DisplayButtonColor
        {
            get { return displayButtonColor; }
            set => Set(ref displayButtonColor, value);
        }

        private string about;
        public string About
        {
            get { return about; }
            set => Set(ref about, value);
        }

        private string name;
        public string Name
        {
            get { return name; }
            set => Set(ref name, value);
        }

        private string email;
        public string Email
        {
            get { return email; }
            set => Set(ref email, value);
        }

        private ImageSource displayIcon;
        public ImageSource DisplayIcon
        {
            get { return displayIcon; }
            set => Set(ref displayIcon, value);
        }


        private ImageSource profileImage;
        public ImageSource ProfileImage
        {
            get { return profileImage; }
            set => Set(ref profileImage, value);
        }

        private bool DisplayOption { get; set; }

        private double deleteButtonOpacity;

        public double DeleteButtonOpacity
        {
            get { return deleteButtonOpacity; }
            set => Set(ref deleteButtonOpacity, value);
        }

        private SelectionMode notificationSelectionMode;
        public SelectionMode NotificationSelectionMode
        {
            get { return notificationSelectionMode; }
            set => Set(ref notificationSelectionMode, value);
        }

        public ProfileViewModel()
        {
            NotificationSelectionMode = SelectionMode.Single;
            DeleteButtonOpacity = 0.3;
            IsDeleting = false;
            Notifications = new ObservableCollection<Notification>();
        }

        
        public async void InitSubscription()
        {
            await semaphoreSlim.WaitAsync();

            var observable = DatabaseService
                .firebase
                .Child("Notificacao")
                .Child(App.user.Key)
                .AsObservable<Notification>();

            Subscription = observable
            .Where(f => !string.IsNullOrEmpty(f.Key))
            .Subscribe(f =>
            {
                if (!IsDeleting)
                {
                    var talk = new Notification
                    {
                        Key = f.Key,
                        Author = f.Object.Author,
                        GroupKey = f.Object.GroupKey,
                        ByteImage = f.Object.ByteImage,
                    };
                    talk.Image = ImageSource.FromStream(() => new MemoryStream(talk.ByteImage));
                    Notifications.Add(talk);
                }
            });

            semaphoreSlim.Release();
        }
        public async void SetProfile()
        {
            App.user = await DatabaseService.GetUserAsync(App.user.Key);

            try
            {
                Email = App.user.Email;
                DisplayOption = App.user.DisplayUserInMap;

                if (App.user.Buffer != null)
                {
                    ProfileImage = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer));
                }
                else
                {
                    App.user.Buffer = ImageService.ConvertToByte("TCCApp.Images.user.png", App.assembly);
                    ProfileImage = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer));
                }

                if (App.user.Sobre != null)
                {
                    About = App.user.Sobre;
                }
                
                if (App.user.Nome != null)
                {
                    Name = App.user.Nome;
                }

                if (App.user.DisplayUserInMap)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayButtonColor = Color.FromHex("#ECEE72");
                        DisplayIcon = "visibility_on.png";
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayButtonColor = Color.FromHex("#FFA8BD");
                        DisplayIcon = "visibility_off.png";

                    });
                }
            }
            catch (Exception)
            {
            }
        }
        public ICommand GoToChat => new Command(async sender =>
        {
            CollectionView view = sender as CollectionView;

            if (view.SelectedItem != null)
            {
                await semaphoreSlim.WaitAsync();

                IsDeleting = true;

                var selected = view.SelectedItem as Notification;

                var chat = await DatabaseService.GetChat(selected.GroupKey);

                if (chat != null)
                {
                    //Se existir a conversa adicione ela na lista de conversa
                    var chatList = new ChatList
                    {
                        Author = selected.Author,
                        ByteImage = selected.ByteImage,
                        GroupKey = selected.GroupKey
                    };
                    DatabaseService.AddToChatList(App.user.Key, chatList);

                    //Delete essa notificacao
                    await DatabaseService.DeleteNotification(selected.Key);

                    //Vou para o chat
                    await Application.Current.MainPage.Navigation.PushAsync(new ChatPage(App.user.Nome, selected.GroupKey));

                    Notifications.Remove(selected);

                    //Limpo a seleção
                    view.SelectedItem = null;

                }
                else
                {
                    //Se a conversa não existir, delete a notificação
                    await Application.Current.MainPage.DisplayAlert("Desculpe", "Parece que a pessoa que te mandou essa notificação mudou de ideia", "ok");
                    await DatabaseService.DeleteNotification(selected.Key);
                    Notifications.Remove(selected);
                }
                IsDeleting = false;
                semaphoreSlim.Release();
            }
        });
        public ICommand DisplayUser => new Command(async() =>
        {
            if (DisplayOption)
            {
                var opt = await Application.Current.MainPage.DisplayAlert("Mudar visibilidade", "Você deseja que os outros usuários não possam te ver no mapa?", "não", "sim");
                if (!opt)
                {

                    DisplayOption = App.user.DisplayUserInMap = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayButtonColor = Color.FromHex("#FFA8BD");
                        DisplayIcon = "visibility_off.png";

                    });
                    
                    await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
                }
            }
            else
            {
                var opt = await Application.Current.MainPage.DisplayAlert("Mudar visibilidade", "Você deseja que os outros usuários possam te ver no mapa?", "não", "sim");
                if (!opt)
                {
                    DisplayOption = App.user.DisplayUserInMap = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayButtonColor = Color.FromHex("#ECEE72");
                        DisplayIcon = "visibility_on.png";
                    });
                    await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
                }
            }
        });
        public ICommand EraseAbout => new Command(async() =>
        {
            var action = await Application.Current.MainPage.DisplayAlert("Apagar sobre", "Tem certeza que deseja apagar o conteúdo sobre você?", "não", "sim");
            if (!action)
            {
                About = string.Empty;
                App.user.Sobre = string.Empty;
            }
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
        });
        public ICommand SetName => new Command(async() =>
        {
            App.user.Nome = Name;
            await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
        });
        public ICommand GetProfileImage => new Command(async() =>
        {
            try
            {
                var media = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions 
                {
                    PhotoSize = PhotoSize.Medium,
                    CompressionQuality = 90,
                });

                var stream = media.GetStream();
                
                var buffer = ImageService.ConvertToByte(stream);
                ProfileImage = ImageSource.FromStream(() => new MemoryStream(buffer));

                App.user.Buffer = buffer;
                await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
            }
            catch (Exception)
            {
            }
        });
        public ICommand SelectMultiple => new Command(async () =>
        {
            if (Notifications.Count > 0)
            {
                //Chatlist em modo de delete
                IsDeleting = true;
                DeleteButtonOpacity = 1;
                NotificationSelectionMode = SelectionMode.Multiple;
                await Application.Current.MainPage.DisplayAlert("Modo delete ativado", "Selecione as notificações que deseja deletar", "ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Você não itens", "Você não tem notificações para deletar", "ok");
            }
        });
        public ICommand DeleteNotification => new Command(async s =>
        {
            await semaphoreSlim.WaitAsync();

            CollectionView collectionView = s as CollectionView;
            var notifications = collectionView.SelectedItems;

            if (notifications != null)
            {
                foreach (Notification notification in notifications)
                {
                    Notifications.Remove(notification);
                    await DatabaseService.DeleteNotification(notification.Key);
                }
            }
            DeleteButtonOpacity = 0.3;
            NotificationSelectionMode = SelectionMode.Single;
            IsDeleting = false;
            semaphoreSlim.Release();
        });
    }
}
