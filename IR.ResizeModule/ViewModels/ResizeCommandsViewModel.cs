using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using IR.Infrastructure.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace IR.ResizeModule.ViewModels
{
    public class ResizeCommandsViewModel: BindableBase
    {
        private readonly IResizeViewModel resizeViewModel;
        private readonly IResizeService resizeService;
        private readonly DelegateCommand resizeFileCommand;
        private readonly DelegateCommand resizeFolderCommand;

        public ResizeCommandsViewModel(IResizeViewModel resizeViewModel, IResizeService resizeService)
        {
            if(resizeViewModel == null)
                throw new ArgumentNullException("resizeViewModel");

            if(resizeService == null)
                throw new ArgumentNullException("resizeService");

            this.resizeViewModel = resizeViewModel;
            this.resizeService = resizeService;
            resizeFileCommand = new DelegateCommand(Resize, CanResize);
            resizeFolderCommand = new DelegateCommand(ResizeFolder, CanResize);

            Observable.FromEventPattern<PropertyChangedEventArgs>(this.resizeViewModel, "PropertyChanged")
                .Subscribe(x =>
                {
                    resizeFileCommand.RaiseCanExecuteChanged();
                    resizeFolderCommand.RaiseCanExecuteChanged();
                });
        }

        private void ResizeFolder()
        {
            var folderBrowserDialog = new FolderBrowserDialog() { ShowNewFolderButton = false};
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                return;

            resizeService.Resize(folderBrowserDialog.SelectedPath, resizeViewModel.MaxWidth, resizeViewModel.MaxHeight, resizeViewModel.Quality, resizeViewModel.SaveSourceImage);
        }

        private bool CanResize()
        {
            return resizeViewModel.MaxHeight != 0
                   && resizeViewModel.MaxWidth != 0
                   && resizeViewModel.Quality > 0
                   && resizeViewModel.Quality <= 100;
        }

        private void Resize()
        {
            var openFileDialog = new OpenFileDialog { RestoreDirectory = false, Multiselect = true };
            if (openFileDialog.ShowDialog() != true)
                return;

            resizeService.Resize(openFileDialog.FileName, resizeViewModel.MaxWidth, resizeViewModel.MaxHeight, resizeViewModel.Quality, resizeViewModel.SaveSourceImage);
        }

        public ICommand ResizeFileCommand { get { return resizeFileCommand; } }

        public ICommand ResizeFolderCommand { get { return resizeFolderCommand; } }
    }
}
