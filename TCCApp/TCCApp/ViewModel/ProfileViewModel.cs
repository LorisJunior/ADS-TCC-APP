using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly:Dependency(typeof(ProfileViewModel))]
namespace TCCApp.ViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        public ObservableCollection<Notification> Notifications { get; private set; }
        public IDisposable Subscription { get; set; }

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

        private ImageSource profileImage;
        public ImageSource ProfileImage
        {
            get { return profileImage; }
            set => Set(ref profileImage, value);
        }

        ChatViewModel chatViewModel;

        public ProfileViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
            DisplayButtonColor = Color.FromHex("#F5BDEF");
            chatViewModel = DependencyService.Get<ChatViewModel>();
            InitChatData();
        }
        public void InitChatData()
        {
            Notifications = new ObservableCollection<Notification>
            {
                new Notification
                {
                    Author = "Lucia",
                    GroupKey = "Conversa1"
                },
                new Notification
                {
                    Author = "Junior",
                    GroupKey = "Conversa2"
                },
                new Notification
                {
                    Author = "Ronaldo",
                    GroupKey = "Conversa3"
                },
            };
        }
        //Todo - Descomentar quando o banco estiver organizado
        /*public async void InitSubscription()
        {
            await semaphoreSlim.WaitAsync();

            var observable = DatabaseService
                .firebase.Child("Notificacao")
                .AsObservable<Notification>();

            Subscription = observable
            .Where(f => !string.IsNullOrEmpty(f.Key)
            //&& f.Object?.Author != Author
            && !string.IsNullOrEmpty(f.Object.Author))
            .Subscribe(f =>
            {
                var talk = new Notification
                {
                    Author = f.Object.Author,
                    GroupKey = f.Object.GroupKey,
                };
                Notifications.Add(talk);
            });

            semaphoreSlim.Release();
        }*/
        public async void SetProfile()
        {
            App.user = await DatabaseService.GetUserAsync(App.user.Key);

            try
            {
                Email = App.user.Email;

                if (App.user.Sobre != null)
                {
                    About = App.user.Sobre;
                }
                if (App.user.Buffer != null)
                {
                    ProfileImage = ImageSource.FromStream(() => new MemoryStream(App.user.Buffer));
                }
                if (App.user.Nome != null)
                {
                    Name = App.user.Nome;
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

                var selected = view.SelectedItem as Notification;

                chatViewModel.Author = App.user.Nome;

                chatViewModel.GroupKey = selected.GroupKey;

                await Application.Current.MainPage.Navigation.PushAsync(new ChatPage());

                view.SelectedItem = null;

                semaphoreSlim.Release();
            }
        });
        public ICommand DisplayUser => new Command(async() =>
        {
            if (App.user.DisplayUserInMap)
            {
                var opt = await Application.Current.MainPage.DisplayAlert("Mudar visibilidade", "Você deseja que os outros usuários não possam te ver no mapa?", "não", "sim");
                if (!opt)
                {

                    App.user.DisplayUserInMap = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayButtonColor = Color.FromHex("#FFA8BD");
                    });
                    
                    await DatabaseService.UpdateUserAsync(App.user.Key, App.user);
                }
            }
            else
            {
                var opt = await Application.Current.MainPage.DisplayAlert("Mudar visibilidade", "Você deseja que os outros usuários possam te ver no mapa?", "não", "sim");
                if (!opt)
                {
                    App.user.DisplayUserInMap = true;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayButtonColor = Color.FromHex("#F5BDEF");
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
    }
}
