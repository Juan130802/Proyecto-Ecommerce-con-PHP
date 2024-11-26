using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class HistorialMedicoPacientesViewModel : BaseViewModel
    {
        private readonly LocalDbService _dbService;
        public ObservableCollection<PacienteHistorial> ListaHistoriales { get; set; } = new ObservableCollection<PacienteHistorial>();

        public HistorialMedicoPacientesViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            _ = CargarHistoriales(1);
        }

        public HistorialMedicoPacientesViewModel() : this(new LocalDbService())
        {
        }
        public async Task CargarHistoriales(int pacienteId)
        {
            var historial = await _dbService.ObtenerHistorialPorId(pacienteId);
            ListaHistoriales.Clear();

            if (historial != null)
            {
                ListaHistoriales.Add(historial);
            }
        }
        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Paciente");
            Application.Current.MainPage = new NavigationPage(new MainPagemenu(userRole));
        }
    }

}
