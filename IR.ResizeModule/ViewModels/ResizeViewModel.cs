using System;
using System.Collections.ObjectModel;
using IR.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace IR.ResizeModule.ViewModels
{
    public class ResizeViewModel: BindableBase, IResizeViewModel
    {
        private readonly ObservableCollection<string> resizedImages;
        private int maxWidth;
        private int maxHeight;
        private int quality;
        private bool saveSourceImage;

        public ResizeViewModel(IResizeService resizeService)
        {
            if(resizeService == null)
                throw new ArgumentNullException("resizeService");

            resizedImages = resizeService.RetrieveResizedImages();
            saveSourceImage = true;
        }

        public int MaxWidth
        {
            get { return maxWidth; }
            set { SetProperty(ref maxWidth, value); }
        }

        public int MaxHeight
        {
            get { return maxHeight; }
            set { SetProperty(ref maxHeight, value); }
        }

        public int Quality
        {
            get { return quality; }
            set { SetProperty(ref quality, value); }
        }

        public bool SaveSourceImage
        {
            get { return saveSourceImage; }
            set { SetProperty(ref saveSourceImage, value); }
        }

        public ObservableCollection<string> ResisedImages { get { return resizedImages; } } 
    }
}
