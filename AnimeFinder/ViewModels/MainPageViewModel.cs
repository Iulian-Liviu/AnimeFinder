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
        public ICommand ItemTappedCommand { get; set; }
        public ICommand LoadMoreAnimes { get; set; }
        #endregion

        #region Private fiels

        private int AnimePages = 0;
        private int CurrentAnimePage;

        #endregion

        #region Methods are here

        private async void GetTopAnimesAsync()
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
            IsAnimeDataLoading = false;
            IsMangaDataLoading = false;
            IsUpComingAnimeDataLoading = false;
        }

        private async void GetTopAnimesByPageAsync()
        {

            // TODO : Solve BUG Loading More items 
            try
            {
                IsAnimeDataLoading = true;

                //Using the RemainingItemsThreshold setted to any value (this will be called n times and will throw an error )
                //TODO : Find a way to solve this 
                var topanime = await jikan.GetTopAnimeAsync(++CurrentAnimePage);


                foreach (var item in topanime.Data)
                {
                    TopAnime.Add(item);
                }

                IsAnimeDataLoading = false;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"{ex.Message}", "ok");
            }
        }


        #endregion
        #region Selecting Items
        private Anime selectedAnime;
        public Anime SelectedAnime
        {
            get => selectedAnime;
            set
            {
                selectedAnime = value;
                RaisePropertyChanged(nameof(SelectedAnime));
            }
        }
        #endregion
        // Here we asign commands 
        void AsignCommands()
        {
            ItemTappedCommand = new Command(ShowToastAlert);
            LoadMoreAnimes = new Command(GetTopAnimesByPageAsync);
        }

        // TODO : Remove this and a real Detail Page
        async void ShowToastAlert()
        {
            if (SelectedAnime is not null)
            {
                await Application.Current.MainPage.DisplayAlert("Selected", $"{SelectedAnime.Title}", "OK");
                SelectedAnime = null;
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
                GetTopAnimesAsync();
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
