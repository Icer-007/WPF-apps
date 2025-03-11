using System.Windows;

namespace Explorlight;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private void CheckBox_GotFocus(object sender, RoutedEventArgs e) => this.tbFilter.Focus();
}
