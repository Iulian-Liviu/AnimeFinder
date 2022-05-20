using AnimeFinder.ViewModels;
using JikanDotNet;

namespace AnimeFinder.Views;


public partial class AnimeDetailPage : ContentPage
{
    public AnimeDetailPage()
    {
        InitializeComponent();
        BindingContext = new AnimeDetailViewModel();
    }

}