using System.Collections.ObjectModel;
using System.ComponentModel;
using Transference.Interface;
using Transference.Models;

namespace Transference.ViewModel
{
    public class OvertimeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<OvertimeModel> _overtimeRecords;
        public ObservableCollection<OvertimeModel> OvertimeRecords
        {
            get => _overtimeRecords;
            set
            {
                if (_overtimeRecords != value)
                {
                    _overtimeRecords = value;
                    OnPropertyChanged(nameof(OvertimeRecords));  // Notifica que la propiedad cambió
                }
            }
        }


        public double TotalHours => OvertimeRecords.Sum(record => record.Hours);

        private readonly IApiService _apiService;

        public OvertimeViewModel(IApiService apiService)
        {
            _apiService = apiService;
            OvertimeRecords = new ObservableCollection<OvertimeModel>();
        }



        // Método para cargar los registros de horas extras de la API
        public async Task LoadOverTime(int userId)
        {
            try
            {
                var response = await _apiService.GetOverTime(userId);

                // Imprimir el tipo de response.Data para depurar
                Console.WriteLine($"Tipo de response.Data: {response.Data.GetType()}");

                // Verificar si la respuesta es exitosa y los datos no son nulos
                //if (response.IsSuccess)
                //{
                //    if (response.Data is IEnumerable<dynamic> dataList)
                //    {
                //        // Si es una colección válida, procesar los datos
                //        foreach (var item in dataList)
                //        {
                //            var overtime = new OvertimeModel
                //            {
                //                UserId = item.userId ?? 0,
                //                AuditCreateDate = item.auditCreateDate ?? DateTime.MinValue,
                //                Hours = item.hours ?? 0.0,
                //                Reason = item.reason ?? string.Empty,
                //                FullName = item.fullName ?? string.Empty
                //            };

                //            OvertimeRecords.Add(overtime);
                //        }
                //    }
                //    else
                //    {
                //        // Si response.Data no es una lista, pero la respuesta es exitosa
                //        Console.WriteLine("Error: La respuesta no contiene una lista válida.");
                //    }
                //}
                //else
                //{
                //    // Si la respuesta no fue exitosa
                //    Console.WriteLine("Error: La respuesta de la API no fue exitosa.");
                //}
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al obtener las horas extras: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}