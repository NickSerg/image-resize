using IR.ResizeModule.ViewModels;
using Microsoft.Practices.Unity;

namespace IR.ResizeModule.Views
{
    /// <summary>
    /// Interaction logic for ResizeView.xaml
    /// </summary>
    public partial class ResizeView
    {
        public ResizeView()
        {
            InitializeComponent();
        }

        [Dependency]
        public IResizeViewModel ViewModel
        {
            get { return DataContext as IResizeViewModel; }
            set
            {
                DataContext = value;
            }
        }
    }
}
