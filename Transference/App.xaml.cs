using Transference.Views;

namespace Transference;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Obtener la vista principal desde el contenedor centralizado en MauiProgram
        var mainPage = MauiProgram.ServiceProvider.GetRequiredService<LoginView>();
        MainPage = new NavigationPage(mainPage);
    }
}
