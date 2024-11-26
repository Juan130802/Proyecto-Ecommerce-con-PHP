using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Preferences = Microsoft.Maui.Storage.Preferences;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class RecetasAsignadasViewModel : BaseViewModel
    {
        private readonly LocalDbService _dbService;
        public ObservableCollection<Recetas> RecetasAsignadas { get; set; } = new ObservableCollection<Recetas>();
        public ICommand IrInicioCommand { get; }
        public ICommand DescargarRecetaCommand { get; }

        public RecetasAsignadasViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            IrInicioCommand = new Command(async () => await IrInicio());
            DescargarRecetaCommand = new Command<Recetas>(async (receta) => await DescargarReceta(receta));
            _ = CargarRecetas();
        }

        public RecetasAsignadasViewModel() : this(new LocalDbService())
        {
        }

        private async Task CargarRecetas()
        {
            string idPacienteStr = Preferences.Get("IdPaciente", "0");
            if (int.TryParse(idPacienteStr, out int idPaciente))
            {
                var recetasAsignadas = await _dbService.GetRecetasPorPaciente(idPaciente);
                RecetasAsignadas.Clear();

                foreach (var receta in recetasAsignadas)
                {
                    RecetasAsignadas.Add(receta);
                }
            }
        }

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Paciente");
            Application.Current.MainPage = new NavigationPage(new MainPagemenu(userRole));
        }

        private async Task DescargarReceta(Recetas receta)
        {
            string ruta = Path.Combine(@"D:\HP\Nueva carpeta\Juan Tareas", $"{receta.NombrePaciente}_Receta.pdf");

            try
            {
                string resultado = await _dbService.GenerarPdfRecetas(receta.IdPaciente, ruta, receta.Id);

                if (!string.IsNullOrEmpty(resultado))
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Receta descargada correctamente.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo descargar la receta.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al intentar descargar la receta: {ex.Message}\n{ex.StackTrace}", "OK");
            }
        }

    }
}
