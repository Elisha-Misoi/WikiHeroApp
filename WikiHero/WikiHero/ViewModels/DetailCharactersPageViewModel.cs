﻿using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
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

namespace WikiHero.ViewModels
{
    public class DetailCharactersPageViewModel : BaseViewModel,INavigationAware
    {
        public ObservableCollection<Character> Characters { get; set; }
        public ObservableCollection<Character> CharactersEnemys { get; set; }
        public ObservableCollection<Comic> Comics { get; set; }
        public ObservableCollection<Movie> Movies { get; set; }
        public ObservableCollection<Volume> Volumes { get; set; }
        public ObservableCollection<Team> Teams { get; set; }
        public Video Video { get; set; }

        public Character Character { get; set; }
        public List<Task> LoadTask { get; set; }
        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand ShareCommand { get; set; }
        public DelegateCommand GoToVideo { get; set; }
        public DetailCharactersPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IApiComicsVine apiComicsVine) : base(navigationService, dialogService, apiComicsVine)
        {
            ShareCommand = new DelegateCommand(async () =>
            {
                await SharedOpcion();
            });
            
            GoToVideo = new DelegateCommand(async () =>
            {
                var param = new NavigationParameters();
                param.Add($"{nameof(Video)}", Video);
                await navigationService.NavigateAsync(new Uri(ConfigPageUri.VideoPage,UriKind.Relative), param);
            });

        }

        async Task LoadCharacter(int id)
        {
            try
            {
                var character = await apiComicsVine.FindCharacter(id, Config.Apikey, null);
                Character = character.Character;
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }
        async Task LoadVideo()
        {
            try
            {
                var videos = await apiComicsVine.GetVideo(Config.Apikey, Character.Name, null);
                foreach (var item in videos.Results.Take(1))
                {
                    Video = item;
                }
              
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }
        async Task LoadMovies(Character character)
        {
            try
            {
                string movie = null;
                foreach (var item in character.Movies)
                {
                    movie += $"{item.Id}|";
                }
                var movies = await apiComicsVine.FindCharactersMovies(Config.Apikey, movie);
                Movies = new ObservableCollection<Movie>(movies.Results.Take(30));

            }
            catch (Exception err)
            {
                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }
        }
        async Task LoadCVolumes(Character character)
        {
            try
            {
                string volume = null;
                foreach (var item in character.VolumeCredits.Take(30))
                {
                    volume += $"{item.Id}|";
                }

                var volumes = await apiComicsVine.FindCharactersVolumes(Config.Apikey, volume);
                Volumes = new ObservableCollection<Volume>(volumes.Volumes);
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }
        async Task LoadCharacterEnemyy(Character character)
        {
            try
            {
                string volume = null;
                foreach (var item in character.CharacterEnemies.Take(30))
                {
                    volume += $"{item.Id}|";
                }

                var characters = await apiComicsVine.FindEnenmyCharacter(Config.Apikey, volume);
                CharactersEnemys = new ObservableCollection<Character>(characters.Characters);
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }
        async Task LoadComics(Character character)
        {
            try
            {
                string comic = null;
                foreach (var item in character.Issues.Take(30))
                {
                    comic += $"{item.Id}|";
                }

                var comics = await apiComicsVine.FindCharacterComics(Config.Apikey, comic);
                Comics = new ObservableCollection<Comic>(comics.Results);
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }

        async Task SharedOpcion()
        {
            await Share.RequestAsync(new ShareTextRequest
            {

                Text = $"{Character.Name}\n{Character.Publisher.Name}\n{Character.Deck}",
                Title = $"{Character.Publisher.Name}\n{Character.Deck}",
                Uri = $"{Character.SiteDetailUrl}"
            });
        }
        async Task LoadTeams(Character character)
        {
            try
            {
                string comic = null;
                foreach (var item in character.Teams.Take(30))
                {
                    comic += $"{item.Id}|";
                }

                var team = await apiComicsVine.FindTeamsCharacter(Config.Apikey, comic);
                Teams = new ObservableCollection<Team>(team.Results);
            }
            catch (Exception err)
            {

                await dialogService.DisplayAlertAsync("Error", $"{err}", "ok");
            }

        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var param = (int)parameters[nameof(Character)];
            var id = param;

            LoadListCommand = new DelegateCommand(async () => {
                await LoadCharacter(id);
                LoadTask = new List<Task>() {LoadVideo(), LoadCharacterEnemyy(Character), LoadCVolumes(Character), LoadMovies(Character), LoadComics(Character), LoadTeams(Character)
                 };
                await Task.WhenAll(LoadTask);
            });
            LoadListCommand.Execute();
        }
    }
}