using JikanDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AnimeFinder.ViewModels;

[QueryProperty(nameof(Manga), "Manga")]
public class MangaDetailViewModel : BaseViewModel
{
    public MangaDetailViewModel()
    {
    }




    Manga manga;
    public Manga Manga
    {
        get => manga;
        set
        {
            manga = value;
            RaisePropertyChanged();
        }
    }
}
