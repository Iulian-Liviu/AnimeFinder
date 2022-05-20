using AnimeFinder.ViewModels;
using JikanDotNet;

namespace AnimeFinder.Views;


public partial class MangaDetailPage : ContentPage
{
    public MangaDetailPage()
    {
        InitializeComponent();
        BindingContext = new MangaDetailViewModel();
    }

}