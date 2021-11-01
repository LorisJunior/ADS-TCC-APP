using System.ComponentModel;
using TCCApp.Helpers;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using Xamarin.Forms;

namespace TCCApp.ViewModel
{
    class TrocarSenhaViewModel : INotifyPropertyChanged
    {
        public User Usuario { get; set; } = new User();
        private Page _page;
        public TrocarSenhaViewModel(Page page, User puser)
        {
            _page = page;
            Usuario = puser;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

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

        private string confirmarsenha;
        public string ConfirmarSenha
        {
            get { return confirmarsenha; }
            set {
                confirmarsenha = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ConfirmarSenha"));
            }
        }

        public Command TrocarSenhaCommand
        {
            get {
                return new Command(() =>
                {
                    if (Usuario.Senha == ConfirmarSenha)
                        TrocarSenha();
                    else
                        App.Current.MainPage.DisplayAlert("", "As senhas não batem", "OK");
                });
            }
        }

        private async void TrocarSenha()
        {
            if (!DatabaseService.IsFormValid(Usuario, _page)) { return; }
            else
            {
                LoadPage.CallLoadingScreen();
                //Chama o método GetUser, que retornará nulo se o email não for encontrado na base de dados
                var user = await DatabaseService.GetUser(Usuario.Email);
                if (user != null)
                {
                    if (Usuario.Email == user.Email)
                    {                        
                        user.Senha = Criptografia.HashValue(Usuario.Senha); ;
                        await DatabaseService.UpdateUserAsync(user.Key, user);
                        await App.Current.MainPage.DisplayAlert("", "Senha trocada com sucesso!", "OK");
                        //Volta para tela inicial
                        await App.Current.MainPage.Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Erro", "OK");
                    LoadPage.CloseLoadingScreen();
                }                    
            }
            
        }
    }
}
