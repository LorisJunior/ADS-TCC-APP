using Newtonsoft.Json;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TCCApp.Helpers;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using Xamarin.Forms;

namespace TCCApp.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public Usuario Usuario { get; set; } = new Usuario();
        private Page _page;
        public event PropertyChangedEventHandler PropertyChanged;
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IOAuth2Service _oAuth2Service;

        public LoginViewModel(Page page, IOAuth2Service oAuth2Service)
        {
            _oAuth2Service = oAuth2Service;
            _page = page;
            OnLoginCommand = new Command<AuthNetwork>(async (data) => await LoginAsync(data));
        }

        private string email;
        public string Email
        {
            get { return email; }
            set {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }
        private string senha;
        public string Senha
        {
            get { return senha; }
            set {
                senha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Senha"));
            }
        }
        public Command LoginCommand
        {
            get {
                return new Command(Login);
            }
        }

        private async void Login()
        {
            //Checa se o email é valído ou se campo email ou senha estão vazios
            if (!UsuarioHelper.IsFormValid(Usuario, _page)){ return; }
            else
            {
                //Chama o método GetUser, que retornará nulo se o email não for encontrado na base de dados
                var user = await UsuarioHelper.GetUser(Usuario.Email);
                if (user != null)
                {
                    if (Usuario.Email == user.Email && Usuario.Senha == user.Senha)
                    {
                        //Abre a tela de HistoryPage após o sucesso do Login    
                        await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Falha no login", "O email ou a senha estão incorretos", "OK");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Falha no login", "Usuário não encontrado", "OK");
            }
        }

        public ICommand OnLoginCommand { get; set; }

        public ObservableCollection<AuthNetwork> AuthenticationNetworks { get; set; } = new ObservableCollection<AuthNetwork>()
        {
            new AuthNetwork()
            {
                Name = "Facebook",
                Icon = "ic_fb",
                Foreground = "#FFFFFF",
                Background = "#4768AD"
            },

            new AuthNetwork()
            {
                Name = "Google",
                Icon = "ic_google",
                Foreground = "#000000",
                Background ="#F8F8F8"
            }
        };
        
        async Task LoginAsync(AuthNetwork authNetwork)
        {
            switch (authNetwork.Name)
            {
                case "Google":
                    await LoginGoogleAsync(authNetwork);
                    break;
            }
        }

        async Task LoginGoogleAsync(AuthNetwork authNetwork)
        {
            try
            {
                if (!string.IsNullOrEmpty(_googleService.AccessToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }

                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
                            #if DEBUG
                            var googleUserString = JsonConvert.SerializeObject(e.Data);
                            Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
                            #endif

                            var socialLoginData = new NetworkAuthData
                            {
                                Id = e.Data.Id,
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = e.Data.Picture.AbsoluteUri,
                                Name = e.Data.Name,
                            };

                            await App.Current.MainPage.Navigation.PushModalAsync(new HistoryPage());
                            break;
                        case GoogleActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }

                    _googleService.OnLogin -= userLoginDelegate;
                };

                _googleService.OnLogin += userLoginDelegate;

                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
