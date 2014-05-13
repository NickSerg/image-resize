using System.Collections.ObjectModel;

namespace IR.Infrastructure.Interfaces
{
    public interface IResizeService
    {
        ObservableCollection<string> RetrieveResizedImages();

        void Resize(string path, int maxWidth, int maxHeight, int quality, bool saveSourceImage = true);
    }
}
