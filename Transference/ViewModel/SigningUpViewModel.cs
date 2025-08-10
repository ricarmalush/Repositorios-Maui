using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Transference.Interface;
using Transference.Models;
using Transference.Shared;
using Transference.Views;

namespace Transference.ViewModel
{
    public class SigningUpViewModel : INotifyPropertyChanged
    {
        private readonly IApiService _apiService;

        // Comandos
        public ICommand InicioJornadaCommand { get; private set; }
        public ICommand PausaAlmuerzoCommand { get; private set; }
        public ICommand RegresoAlmuerzoCommand { get; private set; }
        public ICommand PausaComidaCommand { get; private set; }
        public ICommand RegresoComidaCommand { get; private set; }
        public ICommand FinalizarJornadaCommand { get; private set; }
        public ICommand OtrasOpcionesCommand { get; private set; }
        public ICommand CerrarSesionCommand { get; private set; }

        // Estado de los comandos
        private bool _canExecuteInicioJornadaCommand;
        private bool _canExecutePausaAlmuerzoCommand;
        private bool _canExecuteRegresoAlmuerzoCommand;
        private bool _canExecutePausaComidaCommand;
        private bool _canExecuteRegresoComidaCommand;
        private bool _canExecuteFinalizarJornadaCommand;
        private bool _canExecuteOtrasOpcionesCommand = true;
        private bool _canExecuteCerrarSesionCommand = true;

        public SigningUpViewModel(IApiService apiService)
        {
            _apiService = apiService;

            // Inicializamos los comandos
            InicioJornadaCommand = new Command(async () => await ExecuteCommand(OnInicioJornadaClicked));
            PausaAlmuerzoCommand = new Command(async () => await ExecuteCommand(OnPausaAlmuerzoClicked));
            RegresoAlmuerzoCommand = new Command(async () => await ExecuteCommand(OnRegresoAlmuerzoClicked));
            PausaComidaCommand = new Command(async () => await ExecuteCommand(OnPausaComidaClicked));
            RegresoComidaCommand = new Command(async () => await ExecuteCommand(OnRegresoComidaClicked));
            FinalizarJornadaCommand = new Command(async () => await ExecuteCommand(OnFinalizarJornadaClicked));
            OtrasOpcionesCommand = new Command(async () => await ExecuteCommand(OnOtrasOpcionesClicked));
            CerrarSesionCommand = new Command(async () => await ExecuteCommand(OnCerrarSesionClicked));
        }

        public async Task InitializeAsync()
        {
            await InitializeButtonStatesAsync();
        }

        // Propiedades de CanExecute
        public bool CanExecuteInicioJornadaCommand
        {
            get => _canExecuteInicioJornadaCommand;
            set => SetProperty(ref _canExecuteInicioJornadaCommand, value);
        }

        public bool CanExecutePausaAlmuerzoCommand
        {
            get => _canExecutePausaAlmuerzoCommand;
            set => SetProperty(ref _canExecutePausaAlmuerzoCommand, value);
        }

        public bool CanExecuteRegresoAlmuerzoCommand
        {
            get => _canExecuteRegresoAlmuerzoCommand;
            set => SetProperty(ref _canExecuteRegresoAlmuerzoCommand, value);
        }

        public bool CanExecutePausaComidaCommand
        {
            get => _canExecutePausaComidaCommand;
            set => SetProperty(ref _canExecutePausaComidaCommand, value);
        }

        public bool CanExecuteRegresoComidaCommand
        {
            get => _canExecuteRegresoComidaCommand;
            set => SetProperty(ref _canExecuteRegresoComidaCommand, value);
        }

        public bool CanExecuteFinalizarJornadaCommand
        {
            get => _canExecuteFinalizarJornadaCommand;
            set => SetProperty(ref _canExecuteFinalizarJornadaCommand, value);
        }

        public bool CanExecuteOtrasOpcionesCommand
        {
            get => _canExecuteOtrasOpcionesCommand;
            set => SetProperty(ref _canExecuteOtrasOpcionesCommand, value);
        }

        public bool CanExecuteCerrarSesionCommand
        {
            get => _canExecuteCerrarSesionCommand;
            set => SetProperty(ref _canExecuteCerrarSesionCommand, value);
        }

        private string estadoActual;
        public string EstadoActual
        {
            get => estadoActual;
            set
            {
                estadoActual = value;
                OnPropertyChanged(nameof(EstadoActual));
                // Actualizar TipoFichajeActual cuando cambia EstadoActual
                TipoFichajeActual = TraducirFichajeStatus.FichajeStatus(estadoActual);
            }
        }

        private string tipoFichajeActual;
        public string TipoFichajeActual
        {
            get => tipoFichajeActual;
            private set
            {
                tipoFichajeActual = value;
                OnPropertyChanged(nameof(TipoFichajeActual));
            }
        }



        // Método para ejecutar comandos con manejo de excepciones
        private static async Task ExecuteCommand(Func<Task> command)
        {
            try
            {
                await command();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Métodos para cada acción de los botones
        private async Task OnInicioJornadaClicked()
        {
            await ExecuteWithConfirmation(
                "¿Estás seguro de que deseas iniciar la jornada?",
                async () =>
                {
                    await HandleFichaje("Entrance");
                    CanExecuteInicioJornadaCommand = false;
                }
            );
        }

        private async Task OnPausaAlmuerzoClicked()
        {
            await ExecuteWithConfirmation(
            "¿Estás seguro de que deseas Pausar para Almorzar?",
            async () =>
            {
                await HandleFichaje("LunchBreak");
                CanExecutePausaAlmuerzoCommand = false;
            });                   
        }

        private async Task OnRegresoAlmuerzoClicked()
        {
            await ExecuteWithConfirmation(
                "¿Estás seguro de que deseas Regresar del Almuerzo?",
                async () =>
                {
                    await HandleFichaje("ReturnLunch");
                    CanExecuteRegresoAlmuerzoCommand = false;
                });     
        }

        private async Task OnPausaComidaClicked()
        {
            await ExecuteWithConfirmation(
               "¿Estás seguro de que deseas Pausar para la Comida?",
               async () =>
               {
                   await HandleFichaje("FoodBreak");
                   CanExecutePausaComidaCommand = false;
               });
        }

        private async Task OnRegresoComidaClicked()
        {
            await ExecuteWithConfirmation(
              "¿Estás seguro de que deseas Regresar de la Comida?",
              async () =>
              {
                  await HandleFichaje("ReturnFood");
                  CanExecuteRegresoComidaCommand = false;
              });
        }

        private async Task OnFinalizarJornadaClicked()
        {
              await ExecuteWithConfirmation(
                 "¿Estás seguro de que deseas Finalizar la Jornada?",
                 async () =>
                 {
                     await HandleFichaje("EndDay");
                     CanExecuteFinalizarJornadaCommand = false;
                 });
        }

        private async Task OnOtrasOpcionesClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new OtrasOpciones());
        }

        private async Task ExecuteWithConfirmation(string mensajeConfirmacion, Func<Task> actionToExecute)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", mensajeConfirmacion, "Sí", "No");

            if (confirm)
            {
                // Ejecuta la acción
                await actionToExecute();
            }
        }



        private async Task HandleFichaje(string estado)
        {
            SigningUpModel signingUpModel = new();

            await _apiService.GetActualStateAsync(signingUpModel, estado);
            EstadoActual = signingUpModel.Type;
            await SetButtonStates();
        }

        private static async Task OnCerrarSesionClicked()
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar salida", "¿Estás seguro de que deseas salir de la aplicación?", "Sí", "No");
            if (confirm)
            {
                try
                {
                    SecureStorage.Remove("auth_token");
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Sesión cerrada", "OK");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Método para establecer propiedades y notificar cambios
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName); // Cambia a usar el nombre de la propiedad
            return true;
        }


        private async Task InitializeButtonStatesAsync()
        {
            SetAllButtonStates(false); // Deshabilitar todos los botones inicialmente

            try
            {
                await SetButtonStates();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Método que establece el estado de los botones
        private async Task SetButtonStates()
        {
            string token = await SecureStorage.GetAsync("auth_token"); // Recuperar el token
            var userId = ReturnIdUserToken.GetUserIdFromToken(token); // Obtener el userId del token
            var fichajeStatus = await _apiService.GetActualStateAsync(int.Parse(userId));
            TipoFichajeActual = TraducirFichajeStatus.FichajeStatus(fichajeStatus);

            EstadoActual = fichajeStatus;

            if (fichajeStatus == "NoIniciada")
                CanExecuteInicioJornadaCommand = true;
            else
            {
                // Lógica para los estados específicos
                switch (fichajeStatus)
                {
                    case "Entrance":
                        CanExecutePausaAlmuerzoCommand = true; // Habilita el botón de pausa almuerzo
                        break;
                    case "LunchBreak":
                        CanExecuteRegresoAlmuerzoCommand = true; // Habilita el botón de regreso almuerzo
                        break;
                    case "ReturnLunch":
                        CanExecutePausaComidaCommand = true; // Habilita el botón de pausa comida
                        break;
                    case "FoodBreak":
                        CanExecuteRegresoComidaCommand = true; // Habilita el botón de regreso comida
                        break;
                    case "ReturnFood":
                        CanExecuteFinalizarJornadaCommand = true; // Habilita el botón de finalizar jornada
                        break;
                    case "EndDay":
                        CanExecuteInicioJornadaCommand = true; // Habilita el botón de Inicio de Jornada
                        break;
                    default:
                        SetAllButtonStates(false); // Deshabilita todos si el estado es desconocido
                        break;
                }
            }

            // Boton Cerrar Sesión siempre habilitados
            CanExecuteCerrarSesionCommand = true;
        }

        // Método para habilitar o deshabilitar todos los botones
        private void SetAllButtonStates(bool canExecute)
        {
            CanExecuteInicioJornadaCommand = canExecute;
            CanExecutePausaAlmuerzoCommand = canExecute;
            CanExecuteRegresoAlmuerzoCommand = canExecute;
            CanExecutePausaComidaCommand = canExecute;
            CanExecuteRegresoComidaCommand = canExecute;
            CanExecuteFinalizarJornadaCommand = canExecute;
        }
    }
}
