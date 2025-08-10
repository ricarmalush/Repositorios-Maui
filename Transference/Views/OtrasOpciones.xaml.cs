using Transference.ViewModel;

namespace Transference.Views;

public partial class OtrasOpciones : ContentPage
{
    private Timer _timer; // Utiliza el tipo completo para evitar ambig�edades

    public OtrasOpciones()
    {
        InitializeComponent();
        BindingContext = new OtrasOpcionesViewModel();
        fecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
        StartClock();
    }

    private void StartClock()
    {
        // Inicializa el temporizador para actualizar el reloj cada segundo
        _timer = new Timer(OnTimerElapsed, null, 0, 1000);
    }

    private void OnTimerElapsed(object state)
    {
        // Actualiza el reloj en el hilo principal
        MainThread.BeginInvokeOnMainThread(() =>
        {
            reloj.Text = DateTime.Now.ToString("HH:mm:ss");
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Detenemos el temporizador cuando la p�gina desaparece
        _timer?.Change(Timeout.Infinite, Timeout.Infinite);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Reiniciamos el temporizador cuando la p�gina aparece
        _timer?.Change(0, 1000);
    }

    private async void OnBackgroundImageLoaded(object sender, EventArgs e)
    {
        // Se intenta eliminar el Task.Delay si no es necesario
        loadingImage.IsVisible = false;
        backgroundImage.IsVisible = true;
    }
}