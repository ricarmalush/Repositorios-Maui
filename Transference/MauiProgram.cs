using Transference.Interface;
using Transference.Services;
using Transference.ViewModel;
using Transference.Views;

namespace Transference
{
    public static class MauiProgram
    {
        public static IServiceProvider ServiceProvider { get; private set; } // Necesario para acceder al contenedor de dependencias

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registro de servicios
            builder.Services.AddTransient<HttpClient>(); // Registrar HttpClient
            builder.Services.AddSingleton<IApiService, ApiService>(); // Registro del ApiService

            // Registro de Vistas y ViewModels
            builder.Services.AddTransient<LoginView>(); // Registrar LoginView
            builder.Services.AddTransient<LoginViewModel>(); // Registro del LoginViewModel

            builder.Services.AddTransient<OverTimeView>(); // Registrar OverTimeView
            builder.Services.AddTransient<OvertimeViewModel>(); // Registro del OvertimeViewModel

            builder.Services.AddTransient<SigningUpView>(); // Registrar SigningUpView
            builder.Services.AddTransient<SigningUpViewModel>(); // Registro del SigningUpViewModel

            // Aquí se crea el contenedor de dependencias y se guarda la referencia
            ServiceProvider = builder.Services.BuildServiceProvider();

            return builder.Build();
        }
    }
}
