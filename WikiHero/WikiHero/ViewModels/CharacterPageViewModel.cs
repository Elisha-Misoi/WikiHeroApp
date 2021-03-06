﻿using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiHero.Helpers;
using WikiHero.Models;
using WikiHero.Services;
using Xamarin.Essentials;
using Xamarin.Forms.StateSquid;

namespace WikiHero.ViewModels
{
    public abstract class CharacterPageViewModel : BaseViewModel
    {
        public int ItemTreshold { get; set; }

        public string PublisherName { get; set; }

        public ObservableCollection<Character> Characters { get; set; } = new ObservableCollection<Character>();
        public bool IsChangedView { get; set; } = false;
        public DelegateCommand ChangedViewCommand { get; set; }
        private Character selectCharacter;

        public Character SelectCharacter
        {
            get { return selectCharacter; }
            set { selectCharacter = value;
                if (selectCharacter != null)
                {
                    NavigationToDetail(SelectCharacter);
                }
            }
        }
       
        public CharacterPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IApiComicsVine apiComicsVine,string publisherName,int offeset) : base(navigationService, dialogService, apiComicsVine)
        {
            this.PublisherName = publisherName;
            ChangedViewCommand = new DelegateCommand(()=>{
                IsChangedView = !IsChangedView;
            });
            ItemTresholdReachedCommand = new DelegateCommand(async () =>
            {
                offeset += 100;
                await ScrollLoadCharacters(offeset);
            });

            SearchCommand = new DelegateCommand(async () => 
            {
                await FindCharacter(Text);
            });

            RefreshCommand = new DelegateCommand(async () =>
            {
                IsBusy = true;
                Text = string.Empty;
                await LoadCharacters();
                IsBusy = false;

            });
            LoadListCommand = new DelegateCommand(async () =>
            {
               
                await LoadCharacters();
         
            });
            LoadListCommand.Execute();

        }

        async Task NavigationToDetail(Character character)
        {
            try
            {
                var param = new NavigationParameters
            {
                { nameof(Character), character.Id }
            };
                await navigationService.NavigateAsync(new Uri($"{ConfigPageUri.DetailCharactersPage}",UriKind.Relative), param,true);
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("",$"{err}","ok");
            }

        }
        protected async Task ScrollLoadCharacters(int offset)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    CurrentState = State.Loading;
                    var characters = await apiComicsVine.GetMoreCharacter(Config.Apikey,offset, PublisherName);
                    var notNull = from item in characters.Characters where item.Publisher != null select item;
                    var marvelOrDc = notNull.Where(e => e.Publisher.Name.Contains(PublisherName));
                    foreach (var item in marvelOrDc)
                    {
                        Characters.Add(item);
                    }
                    if (offset == 1000)
                    {
                        ItemTreshold = -1;
                        return;
                    }
                }
                else await dialogService.DisplayAlertAsync("Connection error ", Connectivity.NetworkAccess.ToString(), "Ok");
            }
            catch (Exception ex)
            {
                await dialogService.DisplayAlertAsync("Error", $"{ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
      
        }
      protected async Task LoadCharacters()
        {
            try
            {
                CurrentState = State.Loading;
                var characters = await apiComicsVine.GetCharacter(Config.Apikey, PublisherName);
                var notNull = from item in characters.Characters where item.Publisher != null select item;
                var marvelOrDc = notNull.Where(e => e.Publisher.Name.Contains(PublisherName));
                Characters = new ObservableCollection<Character>(marvelOrDc);
                CurrentState = State.None;
            }
            catch (Exception ex)
            {
                CurrentState = State.Error;
            }
            finally {
                CurrentState = State.None;
            }



        }

        protected async Task FindCharacter(string name) 
        {
            var characters = await apiComicsVine.SearchCharacters(name,Config.Apikey,0,PublisherName);
            var notNull = from item in characters.Characters where item.Publisher != null select item;
            var marvelOrDc = notNull.Where(e => e.Publisher.Name.Contains(PublisherName));
            if (string.IsNullOrEmpty(name))
            {
                await LoadCharacters();
            }
            else
            {

                Characters = new ObservableCollection<Character>(marvelOrDc);
            }
           
        }
    }
}
