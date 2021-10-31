using TCCApp.ViewModel;
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