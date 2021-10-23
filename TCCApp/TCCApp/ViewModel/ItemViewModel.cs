using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.View;
using TCCApp.ViewModel;
using Xamarin.Forms;

[assembly:Dependency(typeof(ItemViewModel))]
namespace TCCApp.ViewModel
{
    public class ItemViewModel
    {
        public ObservableCollection<Item> Items { get; private set; }

        public ItemViewModel()
        {
            Items = new ObservableCollection<Item>();
            InitItems();
        }

        public async void InitItems()
        {
            var items = await DatabaseService.GetItems();
            if (items != null)
            {
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }
        public ICommand DeleteItem => new Command(async s =>
        {
            CollectionView collectionView = s as CollectionView;
            var item = collectionView.SelectedItem as Item;
            Items.Remove(item);

            await DatabaseService.DeleteItemAsync(item.Key);
        });
        public ICommand GoToAddItemPage => new Command(async() =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddItemPage());
        });
    }
}
