using Microsoft.Maui.Controls;

namespace Wrecept.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new MainPage();
    }
}
