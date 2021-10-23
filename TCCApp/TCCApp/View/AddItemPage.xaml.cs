using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.Model;
using TCCApp.Services;
using TCCApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPage : ContentPage
    {
        AddItemViewModel addItemViewModel;
        public AddItemPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            addItemViewModel = DependencyService.Get<AddItemViewModel>();

            BindingContext = addItemViewModel;
        }
    }
}