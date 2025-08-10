using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Transference.Views;

namespace Transference.ViewModel
{
    public class OtrasOpcionesViewModel : INotifyPropertyChanged
    {
        // Comandos
        public ICommand VerFichajesCommand { get; private set; }
        public ICommand PedirVacacionesCommand { get; private set; }
        public ICommand VerVacacionesCommand { get; private set; }
        public ICommand VerHorasExtrasCommand { get; private set; }

        // Estado de los comandos (si quieres hacer que algunos comandos estén habilitados/deshabilitados)
        private bool _canExecuteVerFichajesCommand = true;
        private bool _canExecutePedirVacacionesCommand = true;
        private bool _canExecuteVerVacacionesCommand = true;
        private bool _canExecuteVerHorasExtrasCommand = true;

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public OtrasOpcionesViewModel()
        {
            // Inicializamos los comandos
            VerFichajesCommand = new Command(async () => await ExecuteCommand(OnVerFichajesClicked));
            PedirVacacionesCommand = new Command(async () => await ExecuteCommand(OnPedirVacacionesClicked));
            VerVacacionesCommand = new Command(async () => await ExecuteCommand(OnVerVacacionesClicked));
            VerHorasExtrasCommand = new Command(async () => await ExecuteCommand(OnVerHorasExtrasClicked));
        }

        // Propiedades de CanExecute
        public bool CanExecuteVerFichajesCommand
        {
            get => _canExecuteVerFichajesCommand;
            set => SetProperty(ref _canExecuteVerFichajesCommand, value);
        }

        public bool CanExecutePedirVacacionesCommand
        {
            get => _canExecutePedirVacacionesCommand;
            set => SetProperty(ref _canExecutePedirVacacionesCommand, value);
        }

        public bool CanExecuteVerVacacionesCommand
        {
            get => _canExecuteVerVacacionesCommand;
            set => SetProperty(ref _canExecuteVerVacacionesCommand, value);
        }

        public bool CanExecuteVerHorasExtrasCommand
        {
            get => _canExecuteVerHorasExtrasCommand;
            set => SetProperty(ref _canExecuteVerHorasExtrasCommand, value);
        }

        // Método para establecer propiedades y notificar cambios
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // Evento que notifica los cambios en las propiedades
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        // Acción para el botón "Ver Fichajes"
        private async Task OnVerFichajesClicked()
        {
            try
            {
                //var fichajesView = new FichajesView(); // Crea la vista de fichajes
                ////await Application.Current.MainPage.Navigation.PushAsync(fichajesView);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Acción para el botón "Pedir Vacaciones"
        private async Task OnPedirVacacionesClicked()
        {
            try
            {
                //var vacacionesView = new VacacionesView(); // Crea la vista de pedir vacaciones
                //await Application.Current.MainPage.Navigation.PushAsync(vacacionesView);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Acción para el botón "Ver Vacaciones"
        private async Task OnVerVacacionesClicked()
        {
            try
            {
                //var verVacacionesView = new VerVacacionesView(); // Crea la vista de ver vacaciones
                //await Application.Current.MainPage.Navigation.PushAsync(verVacacionesView);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Acción para el botón "Ver Horas Extras"
        private async Task OnVerHorasExtrasClicked()
        {
            try
            {
                int userId = 1; // Cambia esto por el valor adecuado, si es necesario

                // Obtén el ViewModel desde el contenedor de dependencias
                var overtimeViewModel = MauiProgram.ServiceProvider.GetRequiredService<OvertimeViewModel>();

                // Crea la vista pasando el ViewModel
                var overTimeView = new OverTimeView(overtimeViewModel);

                // Cargar datos usando el ViewModel
                await overtimeViewModel.LoadOverTime(userId);

                // Navegar a la página
                await Application.Current.MainPage.Navigation.PushAsync(overTimeView);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
