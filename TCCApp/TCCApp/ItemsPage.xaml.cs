using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using TCCApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public IItemService ItemDataReference { get; set; }
        public ObservableCollection<Item> MyItems { get; set; }

        public ItemsPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            ItemDataReference = DependencyService.Get<IItemService>();
            ItemData itemsList = (ItemData)ItemDataReference;
            MyItems = new ObservableCollection<Item>(itemsList.Items);
           
            Content.BindingContext = MyItems;
        }

        private async void Adicionar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddItemPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateItemsList();
        }

        public void UpdateItemsList()
        {
            ItemData itemsList = (ItemData)ItemDataReference;
            MyItems = new ObservableCollection<Item>(itemsList.Items);
            itensCollection.ItemsSource = MyItems;
        }

    }
}