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
public class AnimeDetailViewModel : BaseViewModel
{

    public AnimeDetailViewModel()
    {

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
