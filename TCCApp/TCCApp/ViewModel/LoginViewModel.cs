using Newtonsoft.Json;
using Plugin.FacebookClient;
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
        public User Usuario { get; set; } = new User();
        private Page _page;
        public event PropertyChangedEventHandler PropertyChanged;
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        IOAuth2Service _oAuth2Service;
        IFacebookClient _facebookService = CrossFacebookClient.Current;

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
            if (!DatabaseService.IsFormValid(Usuario, _page)){ return; }            
            else
            {
                LoadPage.CallLoadingScreen();
                //Chama o método GetUser, que retornará nulo se o email não for encontrado na base de dados
                var user = await DatabaseService.GetUser(Usuario.Email);
                if (user != null)
                {
                    if (Usuario.Email == user.Email && Criptografia.HashValue(Usuario.Senha) == user.Senha)
                    {
                        //Pega a key do usuário  
                        //App.user.Key = user.Key;
                        //Abre a tela de HistoryPage após o sucesso do Login  
                        await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                        //App.Current.MainPage = new HistoryPage();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Falha no login", "O email ou a senha estão incorretos", "OK");
                        LoadPage.CloseLoadingScreen();
                    }
                        
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Falha no login", "Usuário não encontrado", "OK");
                    LoadPage.CloseLoadingScreen();
                }
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
            LoadPage.CallLoadingScreen();
            switch (authNetwork.Name)
            {
                case "Facebook":
                    await LoginFacebookAsync(authNetwork);
                    break;
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
                            Debug.WriteLine($"Login Google realizado com sucesso: {googleUserString}");
                            #endif

                            var socialLoginData = new NetworkAuthData
                            {
                                Id = e.Data.Id,
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = e.Data.Picture.AbsoluteUri,
                                Name = e.Data.Name,
                                Email = e.Data.Email,
                            };
                            var user = await DatabaseService.GetUser(socialLoginData.Email);
                            if (user != null)
                            {   
                                App.user.Key = user.Key;
                                await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                                //App.Current.MainPage = new HistoryPage();
                            }
                            else
                            {
                                user = new User();
                                user.Email = socialLoginData.Email;
                                user.Nome = socialLoginData.Name;
                                user.Buffer = await ImageService.DownloadImage(socialLoginData.Picture);
                                var adicionado = await DatabaseService.AddUserAsync(user);
                                //Retorno true se o usuário foi inserido com sucesso   
                                if (adicionado)
                                {
                                    user = await DatabaseService.GetUser(socialLoginData.Email);
                                    App.user.Key = user.Key;
                                    await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                                    //App.Current.MainPage = new HistoryPage();
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Erro", "", "OK");
                                    LoadPage.CloseLoadingScreen();
                                }                                    
                            }           
                            break;
                        case GoogleActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Autenticação Google", "Cancelado", "Ok");
                            LoadPage.CloseLoadingScreen();
                            break;
                        case GoogleActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Autenticação Google", "Erro", "Ok");
                            LoadPage.CloseLoadingScreen();
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Autenticação Google", "Não autorizado", "Ok");
                            LoadPage.CloseLoadingScreen();
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

        async Task LoginFacebookAsync(AuthNetwork authNetwork)
        {
            try
            {

                if (_facebookService.IsLoggedIn)
                {
                    _facebookService.Logout();
                }

                EventHandler<FBEventArgs<string>> userDataDelegate = null;

                userDataDelegate = async (object sender, FBEventArgs<string> e) =>
                {
                    switch (e.Status)
                    {
                        case FacebookActionStatus.Completed:
                            var facebookProfile = await Task.Run(() => JsonConvert.DeserializeObject<FacebookProfile>(e.Data));
                            var socialLoginData = new NetworkAuthData
                            {
                                Id = facebookProfile.Id,
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = facebookProfile.Picture.Data.Url,
                                Name = $"{facebookProfile.FirstName} {facebookProfile.LastName}",
                                Email = facebookProfile.Email,
                            };
                            var user = await DatabaseService.GetUser(socialLoginData.Email);
                            if (user != null)
                            {
                                App.user.Key = user.Key;
                                await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                            }
                            else
                            {
                                user = new User();
                                user.Email = socialLoginData.Email;
                                user.Nome = socialLoginData.Name;
                                user.Buffer = await ImageService.DownloadImage(socialLoginData.Picture);
                                var adicionado = await DatabaseService.AddUserAsync(user);
                                //Retorno true se o usuário foi inserido com sucesso   
                                if (adicionado)
                                {
                                    user = await DatabaseService.GetUser(socialLoginData.Email);
                                    App.user.Key = user.Key;
                                    await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Erro", "", "OK");
                                    LoadPage.CloseLoadingScreen();
                                }                                    
                            }
                            break;
                        case FacebookActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Autenticação Facebook", "Cancelado", "Ok");
                            LoadPage.CloseLoadingScreen();
                            break;
                        case FacebookActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Autenticação Facebook", "Erro", "Ok");
                            LoadPage.CloseLoadingScreen();
                            break;
                        case FacebookActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Autenticação Facebook", "Não autorizado", "Ok");
                            LoadPage.CloseLoadingScreen();
                            break;
                    }

                    _facebookService.OnUserData -= userDataDelegate;
                };

                _facebookService.OnUserData += userDataDelegate;

                string[] fbRequestFields = { "email", "first_name", "picture", "gender", "last_name" };
                string[] fbPermisions = { "email" };
                await _facebookService.RequestUserDataAsync(fbRequestFields, fbPermisions);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
