using IR.ResizeModule.ViewModels;
using Microsoft.Practices.Unity;

namespace IR.ResizeModule.Views
{
    /// <summary>
    /// Interaction logic for ResizeCommandsView.xaml
    /// </summary>
    public partial class ResizeCommandsView
    {
        public ResizeCommandsView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ResizeCommandsViewModel ViewModel
        {
            get { return DataContext as ResizeCommandsViewModel; }
            set
            {
                DataContext = value;
            }
        }
    }
}
