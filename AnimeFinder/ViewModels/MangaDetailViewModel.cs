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
public class MangaDetailViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public MangaDetailViewModel()
    {

    }

    private void RaisePropertyChanged([CallerMemberName] string name = " ")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
