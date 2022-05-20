using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AnimeFinder.Views;
using JikanDotNet;

namespace AnimeFinder.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly Jikan jikan = new Jikan();
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // One time check Network Activity
        public MainPageViewModel()
        {
            //Assign commands in below method

            AsignCommands();
            CheckIfInternetIsAvailable();
            RemainingItems = 2;
        }
        #region Data for the view
        private ObservableCollection<Anime> topAnime;
        public ObservableCollection<Anime> TopAnime
        {
            get => topAnime;

            set
            {
                topAnime = value;
                RaisePropertyChanged(nameof(TopAnime));
            }
        }
        private ObservableCollection<Manga> topManga;
        public ObservableCollection<Manga> TopManga
        {
            get => topManga;

            set
            {
                topManga = value;
                RaisePropertyChanged(nameof(TopManga));
            }
        }

        private ObservableCollection<Anime> upcomingAnime;
        public ObservableCollection<Anime> UpComingAnime
        {
            get => upcomingAnime;

            set
            {
                upcomingAnime = value;
                RaisePropertyChanged(nameof(UpComingAnime));
            }
        }

        private bool isMangaDataLoading;
        public bool IsMangaDataLoading
        {
            get => isMangaDataLoading;
            set
            {
                isMangaDataLoading = value;
                RaisePropertyChanged(nameof(IsMangaDataLoading));
            }
        }
        private bool isAnimeDataLoading;
        public bool IsAnimeDataLoading
        {
            get => isAnimeDataLoading;
            set
            {
                isAnimeDataLoading = value;
                RaisePropertyChanged(nameof(IsAnimeDataLoading));
            }
        }

        private bool isUpcomingAnimeDataLoading;
        public bool IsUpComingAnimeDataLoading
        {
            get => isUpcomingAnimeDataLoading;
            set
            {
                isUpcomingAnimeDataLoading = value;
                RaisePropertyChanged(nameof(IsUpComingAnimeDataLoading));
            }
        }

        private ImageSource statusImage;
        public ImageSource StatusImage
        {
            get => statusImage;
            set
            {
                statusImage = value;
                RaisePropertyChanged(nameof(StatusImage));
            }
        }

        #endregion

        #region Commands are here
        public ICommand LoadMoreAnimes { get; set; }
        public ICommand AnimeTappedCommand { get; set; }
        public ICommand UpComingAnimeTappedCommand { get; set; }
        public ICommand MangaTappedCommand { get; set; }
        public ICommand LoadMoreManga { get; set; }

        #endregion

        #region Private fiels

        private int AnimePages, MangaPages, UpComingAnimePages = 0;
        private int CurrentAnimePage, CurrentMangaPage, CurrentUpComingAnimePage;

        #endregion

        #region CollectionView Settings

        private int remainingItems;
        public int RemainingItems
        {
            get => remainingItems;
            set
            {
                remainingItems = value;
                RaisePropertyChanged(nameof(RemainingItems));
            }
        }

        #endregion

        #region Methods are here

        private async void LoadDataOnAppLaunch()
        {
            try
            {

                IsAnimeDataLoading = true;
                IsMangaDataLoading = true;
                isUpcomingAnimeDataLoading = true;

                var topanime = await jikan.GetTopAnimeAsync();
                var topmanga = await jikan.GetTopMangaAsync();
                var upcoming = await jikan.GetUpcomingSeasonAsync();

                TopAnime = new ObservableCollection<Anime>(topanime.Data);
                TopManga = new ObservableCollection<Manga>(topmanga.Data);
                UpComingAnime = new ObservableCollection<Anime>(upcoming.Data);

                AnimePages = topanime.Pagination.LastVisiblePage;
                CurrentAnimePage = topanime.Pagination.CurrentPage;

                MangaPages = topmanga.Pagination.LastVisiblePage;
                CurrentMangaPage = topmanga.Pagination.CurrentPage;

                UpComingAnimePages = upcoming.Pagination.LastVisiblePage;
                CurrentUpComingAnimePage = upcoming.Pagination.CurrentPage;

                IsAnimeDataLoading = false;
                IsMangaDataLoading = false;
                IsUpComingAnimeDataLoading = false;
            }
            catch (global::System.Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Connection Erorr", $"Exception : {ex.Message}", "Ok");
            }
        }

        private async void GetTopAnimesByPageAsync()
        {

            // TODO : Solve BUG Loading More items : Solved 
            try
            {
                if (CurrentAnimePage <= AnimePages)
                {
                    await LoadMoreAnimes();
                }
                else
                {
                    await DisplayMessage("Slow down", "There are no more popular animes to show!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayMessage("Connection Error", $"Error : {ex.Message}", "OK");
            }

            async Task LoadMoreAnimes()
            {
                RemainingItems = -1;
                CurrentAnimePage = CurrentAnimePage + 1;
                await Task.Run(async () =>
                {
                    IsAnimeDataLoading = true;

                    //Using the RemainingItemsThreshold setted to any value (this will be called n times and will throw an error )
                    //TODO : Find a way to solve this : Solved
                    var topanime = await jikan.GetTopAnimeAsync(CurrentAnimePage);


                    foreach (var item in topanime.Data)
                    {
                        TopAnime.Add(item);
                    }
                    IsAnimeDataLoading = false;
                });
                RemainingItems = 2;

            }
        }

        private async void GetTopMangaByPageAsync()
        {

            // TODO : Solve BUG Loading More items 
            try
            {
                if (CurrentMangaPage <= MangaPages)
                {
                    await LoadMoreMangas();
                }
                else
                {
                    await DisplayMessage("Slow down", "There are no more popular animes to show!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayMessage("Connection Error", $"Error : {ex.Message}", "OK");
            }

            async Task LoadMoreMangas()
            {
                RemainingItems = -1;
                CurrentMangaPage = CurrentMangaPage + 1;
                await Task.Run(async () =>
                {
                    IsMangaDataLoading = true;

                    //Using the RemainingItemsThreshold setted to any value (this will be called n times and will throw an error )
                    //TODO : Find a way to solve this 
                    var topmanga = await jikan.GetTopMangaAsync(CurrentMangaPage);


                    foreach (var item in topmanga.Data)
                    {
                        TopManga.Add(item);
                    }
                    IsMangaDataLoading = false;
                });

                RemainingItems = 2;

            }
        }

        /*private async void GetTopUpComingAnimeByPageAsync()
        {

            // TODO : Solve BUG Loading More items 
            try
            {
                if (CurrentUpComingAnimePage <= UpComingAnimePages)
                {
                    await LoadMoreUpComingAnime();
                }
                else
                {
                    await DisplayMessage("Slow down", "There are no more popular animes to show!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayMessage("Connection Error", $"Error : {ex.Message}", "OK");
            }

            async Task LoadMoreUpComingAnime()
            {
                RemainingItems = -1;
                CurrentUpComingAnimePage = CurrentUpComingAnimePage + 1;
                await Task.Run(async () =>
                {
                    IsMangaDataLoading = true;

                    //Using the RemainingItemsThreshold setted to any value (this will be called n times and will throw an error )
                    //TODO : Find a way to solve this 
                    var topmanga = await jikan.GetUpcomingSeasonAsync(CurrentUpComingAnimePage);


                    foreach (var item in topmanga.Data)
                    {
                        TopManga.Add(item);
                    }
                    IsMangaDataLoading = false;
                });

                RemainingItems = 2;

            }
        }
*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1">Title</param>
        /// <param name="v2">Message</param>
        /// <param name="v3">Action</param>
        /// <returns></returns>
        private async Task DisplayMessage(string v1, string v2, string v3)
        {
            await Application.Current.MainPage.DisplayAlert(v1, v2, v3);
        }


        #endregion


        // Here we asign commands 
        void AsignCommands()
        {
            LoadMoreAnimes = new Command(GetTopAnimesByPageAsync);
            LoadMoreManga = new Command(GetTopMangaByPageAsync);
            AnimeTappedCommand = new Command<Anime>(DisplayAnimePage);
            MangaTappedCommand = new Command<Manga>(DisplayMangaPage);
            UpComingAnimeTappedCommand = new Command<Anime>(DisplayUpComingAnimePage);
        }

        async void DisplayAnimePage(Anime anime)
        {
            if (anime != null)
            {
                // TODO : Implement a  real Detail page

                //await DisplayMessage("Works", $"{anime.Title}", "Ok");



                await Shell.Current.GoToAsync(nameof(AnimeDetailPage), new Dictionary<string, object>
                {
                    { "Anime", anime },
                });

            }
        }
        async void DisplayMangaPage(Manga manga)
        {
            if (manga != null)
            {
                // TODO : Implement a  real Detail page

                // await DisplayMessage("Works", $"{manga.Title}", "Ok");

                await Shell.Current.GoToAsync(nameof(MangaDetailPage), new Dictionary<string, object>
                {
                    { "Manga", manga }
                });

            }
        }
        async void DisplayUpComingAnimePage(Anime anime)
        {
            if (anime != null)
            {   // TODO : Implement a  real Detail page
                await DisplayMessage("Works", $"{anime.Title}", "Ok");
            }
        }

        // NEED CHECKING
        #region Internet Check
        private bool isInternetAvailable;
        public bool IsInternetAvailable
        {
            get => isInternetAvailable;
            set
            {
                isInternetAvailable = value;
                RaisePropertyChanged(nameof(IsInternetAvailable));
            }
        }


        // TODO : FIND A WAY TO CHECK IF NETWORK IS AVAILABLE (ALWAYS)
        // Now just work only at opening the app
        void CheckIfInternetIsAvailable()
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                StatusImage = ImageSource.FromFile("internet.png");
                IsInternetAvailable = true;
                LoadDataOnAppLaunch();
            }
            else
            {
                IsInternetAvailable = false;
                StatusImage = ImageSource.FromFile("no_internet.png");
            }
        }
        #endregion
    }
}
