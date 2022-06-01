using JikanDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AnimeFinder.ViewModels;

public class AnimeDetailViewModel : BaseViewModel, IQueryAttributable
{

    public AnimeDetailViewModel()
    {

    }

    public Anime Anime { get; private set; } = new Anime();

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Anime = query["Anime"] as Anime;
        RaisePropertyChanged("Anime");
    }

    private bool showKnownAs;
    public bool ShowKnownAs
    {
        get => showKnownAs;

        set
        {
            showKnownAs = value;
            RaisePropertyChanged();
        }
    }


}
