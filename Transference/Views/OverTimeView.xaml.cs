using Transference.ViewModel;

namespace Transference.Views;

public partial class OverTimeView : ContentPage
{
    private readonly OvertimeViewModel _viewModel;

    public OverTimeView(OvertimeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;  // Asegúrate de asignar el BindingContext correctamente
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadOverTime(1);  // Llamada a la API para cargar los datos (por ejemplo, con un userId)
    }
}