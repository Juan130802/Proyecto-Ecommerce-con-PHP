using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class CitasAgendadasUsuarioViewModel : BaseViewModel
    {
        private readonly LocalDbService _dbService;
        public ObservableCollection<Usuarios> CitasUsuario { get; set; } = new ObservableCollection<Usuarios>();
        public ICommand IrInicioCommand { get; }

        public CitasAgendadasUsuarioViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            IrInicioCommand = new Command(async () => await IrInicio());
            _ = CargarCitas();
        }

        public CitasAgendadasUsuarioViewModel() : this(new LocalDbService())
        {
            IrInicioCommand = new Command(async () => await IrInicio());
        }

        private async Task CargarCitas()
        {
            string idPacienteStr = Preferences.Get("IdPaciente", "0");
            if (int.TryParse(idPacienteStr, out int idPaciente))
            {
                // Obtener las citas del paciente y la lista de doctores
                var citasUsuario = await _dbService.GetCitasPorIdPaciente(idPaciente);
                var doctores = await _dbService.GetDoctores();
                var doctoresOrdenados = doctores.OrderBy(d => d.Name).ToList();

                if (doctoresOrdenados.Count == 0)
                {
                    return;
                }

                // Asignar doctor a cada cita que no tenga doctor asignado
                foreach (var cita in citasUsuario)
                {
                    if (string.IsNullOrWhiteSpace(cita.DoctorAsignado) || cita.Id == 0)
                    {
                        foreach (var doctor in doctoresOrdenados)
                        {
                            int citasDelDoctor = citasUsuario.Count(c => c.Id == doctor.Id && c.Fecha.Date == cita.Fecha.Date);

                            if (citasDelDoctor < 5) // Asignar si el doctor tiene menos de 5 citas ese día
                            {
                                cita.DoctorAsignado = doctor.Name;
                                cita.Id = doctor.Id;

                                // Actualizar en la base de datos
                                await _dbService.Update(cita);
                                break;
                            }
                        }
                    }
                }

                // Actualizar la colección para mostrar en la vista
                CitasUsuario.Clear();
                foreach (var cita in citasUsuario)
                {
                    CitasUsuario.Add(cita);
                }
            }
        }

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Paciente");
            Application.Current.MainPage = new NavigationPage(new MainPagemenu(userRole));
        }
    }
}
