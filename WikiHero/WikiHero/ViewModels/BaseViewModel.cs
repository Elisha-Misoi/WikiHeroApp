﻿using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WikiHero.Services;
using Xamarin.Essentials;
using Xamarin.Forms.StateSquid;

namespace WikiHero.ViewModels
{
    public abstract class BaseViewModel:INotifyPropertyChanged
    {
        protected INavigationService navigationService;
        protected IPageDialogService dialogService;
        protected IApiComicsVine apiComicsVine;
        public DelegateCommand RefreshCommand { get; set; }
        public string Text { get; set; }
        public bool IsBusy { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand ItemTresholdReachedCommand { get; set; }
        public DelegateCommand LoadListCommand { get; set; }
        public State CurrentState { get; set; }
        public bool IsConnected { get; set; }
        public BaseViewModel(INavigationService navigationService, IPageDialogService dialogService, IApiComicsVine apiComicsVine)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
            this.apiComicsVine = apiComicsVine;
            IsConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
