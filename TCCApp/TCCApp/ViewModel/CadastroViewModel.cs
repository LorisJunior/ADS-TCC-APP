using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using Xamarin.Forms;

namespace TCCApp.ViewModel
{
    class CadastroViewModel : INotifyPropertyChanged
    {
        public User Usuario { get; set; } = new User();
        private Page _page;
        public CadastroViewModel(Page page)
        {
            _page = page;
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

        public event PropertyChangedEventHandler PropertyChanged;

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
        public Command CadastrarCommand
        {
            get {
                return new Command(() =>
                {
                    if (Usuario.Senha == ConfirmarSenha)
                        Cadastrar();
                    else
                        App.Current.MainPage.DisplayAlert("", "As senhas não batem", "OK");
                });
            }
        }
        private async void Cadastrar()
        {
            if (!DatabaseService.IsFormValid(Usuario, _page)) { return; }
            else
            {
                //Chama o método GetUser, que retornará nulo se o email não for encontrado na base de dados
                var novoUser = await DatabaseService.GetUser(Usuario.Email);
                if (novoUser == null)
                {
                    //Chama o método AddUser, que irá inserir o usuário no base de dados
                    var user = await DatabaseService.AddUserAsync(Usuario);
                    //Retorno true se o usuário foi inserido com sucesso   
                    if (user)
                    {
                        await App.Current.MainPage.DisplayAlert("Successo!", "", "Ok");
                        //Abre a tela de HistoryPage após o sucesso do Login
                        await App.Current.MainPage.Navigation.PushAsync(new HistoryPage());
                    }
                    else
                        await App.Current.MainPage.DisplayAlert("Erro", "", "OK");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Erro", "Esse usuário já existe.", "OK");
            }
        }
    }
}
