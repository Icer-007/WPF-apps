using Explorlight.ViewModels;
using System.Windows;
using System.Windows.Threading;

namespace Explorlight;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
    }

    ~App()
    {
        this.DispatcherUnhandledException -= this.App_DispatcherUnhandledException;
    }

    public void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // Process unhandled exception
        MessageBox.Show(e.Exception.ToString(), "Unexpected exception", MessageBoxButton.OK, MessageBoxImage.Error);

        // Prevent default unhandled exception processing
        e.Handled = true;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        new MainWindow { DataContext = new MainViewModel() }.Show();
    }
}
