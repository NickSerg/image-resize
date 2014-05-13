using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

namespace ImageResize
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private Bootstrapper bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            Current.DispatcherUnhandledException += ApplicationOnDispatcherUnhandledException;
        }

        private void ApplicationOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            bootstrapper.Container.Resolve<ILoggerFacade>().Log(dispatcherUnhandledExceptionEventArgs.Exception.Message, Category.Exception, Priority.High);
            dispatcherUnhandledExceptionEventArgs.Handled = true;
        }
    }
}
