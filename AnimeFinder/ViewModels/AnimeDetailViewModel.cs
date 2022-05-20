using JikanDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AnimeFinder.ViewModels;

[QueryProperty(nameof(Anime), "Anime")]
public class AnimeDetailViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public AnimeDetailViewModel()
    {

    }

    private void RaisePropertyChanged([CallerMemberName] string name = " ")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    Anime anime;
    public Anime Anime
    {
        get => anime;
        set
        {
            anime = value;
            RaisePropertyChanged();
        }
    }
}
