using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IR.ResizeModule.ViewModels
{
    public interface IResizeViewModel
    {
        int MaxWidth { get; set; }

        int MaxHeight { get; set; }

        int Quality { get; set; }

        bool SaveSourceImage { get; set; }

        ObservableCollection<string> ResisedImages { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}