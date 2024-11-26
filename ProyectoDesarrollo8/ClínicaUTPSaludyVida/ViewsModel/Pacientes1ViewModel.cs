using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public partial class Pacientes1ViewModel : BaseViewModel
    {
        private readonly LocalDbService _dbService;
        public ObservableCollection<PacientesRegister> ListaPacientes { get; set; } = new ObservableCollection<PacientesRegister>();

        public ICommand PacienteTappedCommand { get; }
        public ICommand VerCitasCommand { get; }

        public Pacientes1ViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            PacienteTappedCommand = new Command<PacientesRegister>(async (paciente) => await MostrarOpciones(paciente));
            VerCitasCommand = new Command<PacientesRegister>(async (paciente) => await VerCitas(paciente));
            _ = CargarPacientes();
        }

        public Pacientes1ViewModel() : this(new LocalDbService())
        {
        }

        private async Task CargarPacientes()
        {
            var pacientes = await _dbService.GetPacienteRegis();
            ListaPacientes.Clear();
            foreach (var paciente in pacientes)
            {
                ListaPacientes.Add(paciente);
            }
        }

        private async Task MostrarOpciones(PacientesRegister paciente)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Acción", "Cancelar", null, "Eliminar", "Actualizar");

            if (action == "Eliminar")
            {
                bool confirmacion = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas eliminar este paciente?", "Sí", "No");
                if (confirmacion)
                {
                    await _dbService.Delete(paciente);
                    ListaPacientes.Remove(paciente);
                }
            }
            else if (action == "Actualizar")
            {
                var editarPacientePage = new Pacientes(new PacientesViewModel(paciente));
                await Application.Current.MainPage.Navigation.PushAsync(editarPacientePage);
            }
        }

        private async Task VerCitas(PacientesRegister paciente)
        {
            Preferences.Set("IdPaciente", paciente.Id.ToString());
            await Application.Current.MainPage.Navigation.PushAsync(new CitasAgendadasUsuario(new LocalDbService()));
        }
    }
}
