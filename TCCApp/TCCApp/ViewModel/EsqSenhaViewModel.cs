using System.ComponentModel;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using Xamarin.Forms;

namespace TCCApp.ViewModel
{
    class EsqSenhaViewModel : INotifyPropertyChanged
    {
        public User Usuario { get; set; } = new User();
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

        public Command EnviarEmailCommand
        {
            get {
                return new Command(EnviarEmail);
            }
        }

        private async void EnviarEmail()
        {
            LoadPage.CallLoadingScreen();
            //Chama o método GetUser, que retornará nulo se o email não for encontrado na base de dados
            var user = await DatabaseService.GetUser(Usuario.Email);
            if (user != null)
            {
                if (Usuario.Email == user.Email)
                {
                    //Abre a tela de HistoryPage após o sucesso do Login   
                    await App.Current.MainPage.Navigation.PushAsync(new TrocarSenhaPage(Usuario));
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "O email inserido não existe na base de dados", "OK");
                LoadPage.CloseLoadingScreen();
            }                
        }
    }
}
