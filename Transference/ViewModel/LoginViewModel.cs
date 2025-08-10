using System.Windows.Input;
using Transference.Interface;
using Transference.Shared;
using Transference.ViewModel;
using Transference.Views;

namespace Transference
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private string _usuario;
        private string _password;
        private bool _isLoggedIn;

        // Propiedad para el estado de carga
        private bool _isBusy;  // Nueva propiedad para manejar el estado de carga
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand LoginCommand { get; }

        public LoginViewModel(IApiService apiService)
        {
            _apiService = apiService;

            // Inicializa el comando en el constructor
            LoginCommand = new Command(async () => await LoginAsync(), CanLogin);
        }

        // Propiedad para el estado de inicio de sesión
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    OnPropertyChanged();
                }
            }
        }

        // Propiedad para el nombre de usuario
        public string Usuario
        {
            get => _usuario;
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    OnPropertyChanged();
                    ((Command)LoginCommand).ChangeCanExecute(); // Actualiza el estado del comando
                }
            }
        }

        // Propiedad para la contraseña
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    ((Command)LoginCommand).ChangeCanExecute(); // Actualiza el estado del comando
                }
            }
        }

        // Método que determina si se puede ejecutar el comando
        private bool CanLogin() => !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Password);

        // Método para manejar el inicio de sesión
        private async Task LoginAsync()
        {
            // Validación de usuario y contraseña
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                await App.Current.MainPage.DisplayAlert("Advertencia", DiccionaryMessage.Credentials, "OK");
                return;
            }

            IsBusy = true; // Muestra el indicador de carga

            try
            {
                var result = await _apiService.LoginAsync(Usuario, Password, "Interno");
                IsLoggedIn = result;

                if (!result)
                    await App.Current.MainPage.DisplayAlert("Advertencia", DiccionaryMessage.Credentials, "OK");
                else
                {
                    // Obtener el servicio de navegación y ViewModel desde el contenedor
                    var mauiContext = Application.Current.Handler?.MauiContext;

                    if (mauiContext != null)
                    {
                        var serviceProvider = mauiContext.Services;

                        // Obtener ApiService y SigningUpViewModel desde el contenedor de servicios
                        var signingUpViewModel = serviceProvider.GetService<SigningUpViewModel>();

                        await signingUpViewModel.InitializeAsync();

                        // Crear la vista y pasarle el ViewModel
                        var signingUpView = new SigningUpView(signingUpViewModel);

                        // Navegar a la nueva vista
                        await Application.Current.MainPage.Navigation.PushAsync(signingUpView);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores, si es necesario
                await App.Current.MainPage.DisplayAlert("Error", DiccionaryMessage.ErrorSesion, "OK");
            }
            finally
            {
                IsBusy = false; // Oculta el indicador de carga
            }
        }
    }
}
