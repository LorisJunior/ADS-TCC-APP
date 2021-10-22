using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ItemViewModel))]
namespace TCCApp.ViewModel
{
    class ItemViewModel
    {
        public ObservableCollection<Item> Items { get; private set; }

        public ItemViewModel()
        {
            Items = new ObservableCollection<Item>
            {
                new Item
                {
                    Nome = "Camisa",
                    Cor = Color.FromHex("#BDF5F5"),
                    ImageUrl = "camisaIcon.png",
                    Descricao = "test",
                    Quantidade = 5
                },

                new Item
                {
                    Nome = "Shorts",
                    Cor = Color.FromHex("#F5BDEF"),
                    ImageUrl = "shorts.png",
                    Descricao = "",
                    Quantidade = 6
                },

                new Item
                {
                    Nome = "Garrafa",
                    Cor = Color.FromHex("#EDF5BD"),
                    ImageUrl = "garrafaIcon.png",
                    Descricao = "",
                    Quantidade = 2
                }
            };
        }
        public ICommand DeleteItem => new Command(s =>
        {
            CollectionView collectionView = s as CollectionView;
            var item = collectionView.SelectedItem as Item;
            Items.Remove(item);
        });
        public ICommand GoToAddItemPage => new Command(async() =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddItemPage());
        });
    }
}
