﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCCApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TCCApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EsqSenhaPage : ContentPage
    {
        EsqSenhaViewModel esqSenhaViewModel;
        public EsqSenhaPage()
        {
            esqSenhaViewModel = new EsqSenhaViewModel();
            InitializeComponent();
            BindingContext = esqSenhaViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}