using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {

        ItemViewModel itemViewModel;
        public ItemsPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            itemViewModel = DependencyService.Get<ItemViewModel>();
           
            BindingContext = itemViewModel;

        }
    }
}