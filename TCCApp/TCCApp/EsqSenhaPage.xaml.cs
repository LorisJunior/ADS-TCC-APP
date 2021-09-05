using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EsqSenhaPage : ContentPage
    {
        public EsqSenhaPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}