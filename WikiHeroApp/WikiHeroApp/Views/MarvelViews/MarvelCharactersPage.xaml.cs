﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikiHeroApp.Views.MarvelViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarvelCharactersPage : ContentPage
    {
        public MarvelCharactersPage()
        {
            InitializeComponent();
        }
    }
}