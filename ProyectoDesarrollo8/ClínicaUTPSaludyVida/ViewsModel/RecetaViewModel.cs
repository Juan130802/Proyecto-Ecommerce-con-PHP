using Clínica_UTP_Salud_y_Vida.Models;
using MvvmHelpers;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class RecetaViewModel : BaseViewModel
    {
        private readonly LocalDbService _dbService;

        public string NombreDoctor { get; set; }
        public string Especialidad { get; set; }

        // Listas para manejar hasta 4 medicamentos y dosis
        public List<string> Medicamentos { get; set; } = new List<string> { "", "", "", "" };
        public List<string> Dosis { get; set; } = new List<string> { "", "", "", "" };

        public int IdPaciente { get; set; }
        public string NombrePaciente { get; set; }

        public ICommand GuardarRecetaCommand { get; }

        public RecetaViewModel(LocalDbService dbService)
        {
            _dbService = dbService;

            string idPacienteStr = Preferences.Get("IdPaciente", "0");
            if (int.TryParse(idPacienteStr, out int idPaciente))
            {
                IdPaciente = idPaciente;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Error", "No se ha encontrado un ID de paciente válido.", "OK");
                return;
            }

            GuardarRecetaCommand = new Command(async () => await GuardarReceta());
        }

        public RecetaViewModel() : this(new LocalDbService())
        {
        }

        private async Task GuardarReceta()
        {
            string medicamentosConcatenados = string.Join(",", Medicamentos.Where(m => !string.IsNullOrWhiteSpace(m)));
            string dosisConcatenadas = string.Join(",", Dosis.Where(d => !string.IsNullOrWhiteSpace(d)));

            var nuevaReceta = new Recetas
            {
                NombreDoctor = NombreDoctor,
                Especialidad = Especialidad,
                Medicamento = medicamentosConcatenados,
                Dosis = dosisConcatenadas,
                IdPaciente = IdPaciente,
                NombrePaciente = NombrePaciente
            };

            await _dbService.CreateReceta(nuevaReceta);

            await Application.Current.MainPage.DisplayAlert("Éxito", "La receta ha sido guardada correctamente.", "OK");
        }
    }
}
