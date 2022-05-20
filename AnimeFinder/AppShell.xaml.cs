using AnimeFinder.Views;

namespace AnimeFinder;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(AnimeDetailPage), typeof(AnimeDetailPage));
        Routing.RegisterRoute(nameof(MangaDetailPage), typeof(MangaDetailPage));

    }


}