
namespace Transference.Shared
{
    public static class TraducirFichajeStatus
    {
        public static string FichajeStatus(string fichajeStatus)
        {
            // Si el estado de fichaje está vacío o es nulo (base de datos vacía), comenzamos en "Entrance"
            if (fichajeStatus == "NoIniciada")
                return "Inicio Jornada";

            // Diccionario para mapear cada estado al siguiente estado
            var nextFichaje = new Dictionary<string, string>
            {
                { "Entrance", "LunchBreak" },
                { "LunchBreak", "ReturnLunch" },
                { "ReturnLunch", "FoodBreak" },
                { "FoodBreak", "ReturnFood" },
                { "ReturnFood", "EndDay" },
                { "EndDay", "Entrance" }
            };

            // Diccionario para obtener la etiqueta de cada estado
            var fichajeLabels = new Dictionary<string, string>
            {
                { "Entrance", "Inicio Jornada" },
                { "EndDay", "Finalizar Jornada" }, // Cambiado para reflejar la lógica correcta
                { "LunchBreak", "Pausa Almuerzo" },
                { "ReturnLunch", "Regreso Almuerzo" },
                { "FoodBreak", "Pausa Comida" },
                { "ReturnFood", "Regreso Comida" }
            };

            // Obtener la etiqueta del estado actual
            if (!fichajeLabels.TryGetValue(fichajeStatus, out string currentLabel))
                return "Estado Desconocido";

            // Obtener el siguiente estado en el ciclo
            if (!nextFichaje.TryGetValue(fichajeStatus, out string nextStatus))
                return "Estado Desconocido";

            // Obtener la etiqueta del siguiente estado
            string nextLabel = fichajeLabels[nextStatus];

            return $"Estado Actual: {currentLabel}, Próximo: {nextLabel}";
        }
    }

    }
