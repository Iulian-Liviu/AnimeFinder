using AnimeFinder.Views;
namespace AnimeFinder;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}
