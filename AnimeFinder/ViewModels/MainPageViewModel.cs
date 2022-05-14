using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainPageViewModel()
        {
            AsignCommands();
            IsDataLoading = false;
            CheckIfInternetIsAvailable();
        }

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


        private bool isDataLoading;
        public bool IsDataLoading
        {
            get => isDataLoading;
            set
            {
                isDataLoading = value;
                RaisePropertyChanged(nameof(IsDataLoading));
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


        public ICommand ItemTappedCommand { get; set; }

        private async void GetTopAnimesAsync()
        {
            IsDataLoading = true;
            var jikan = new Jikan();

            var topanime = await jikan.GetTopAnimeAsync();
            var topmanga = await jikan.GetTopMangaAsync();
            var upcoming = await jikan.GetUpcomingSeasonAsync();

            TopAnime = new ObservableCollection<Anime>(topanime.Data);
            TopManga = new ObservableCollection<Manga>(topmanga.Data);
            UpComingAnime = new ObservableCollection<Anime>(upcoming.Data);
            IsDataLoading = false;
        }

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

        void AsignCommands()
        {
            ItemTappedCommand = new Command(ShowToastAlert);
        }


        async void ShowToastAlert()
        {
            if (SelectedAnime is not null)
            {
                await Application.Current.MainPage.DisplayAlert("Selected", $"{SelectedAnime.Title}", "OK");
                SelectedAnime = null;
            }

        }

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

        void CheckIfInternetIsAvailable()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
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

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
