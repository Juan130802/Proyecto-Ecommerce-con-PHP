using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class CitasAgendadasDoctorViewModel : BaseViewModel
    {
        private readonly LocalDbService _dbService;
        public ObservableCollection<Usuarios> CitasDoctor { get; set; } = new ObservableCollection<Usuarios>();
        public ICommand IrInicioCommand { get; }

        public CitasAgendadasDoctorViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            IrInicioCommand = new Command(async () => await IrInicio());
            _ = CargarCitasAsync();
        }

        public CitasAgendadasDoctorViewModel() : this(new LocalDbService())
        {
            IrInicioCommand = new Command(async () => await IrInicio());
        }

        private async Task CargarCitasAsync()
        {
            string doctorIdStr = Preferences.Get("IdDoctor", "0");
            if (int.TryParse(doctorIdStr, out int doctorId))
            {
                var citas = await _dbService.GetCitasPorDoctorId(doctorId);

                CitasDoctor.Clear();
                foreach (var cita in citas)
                {
                    CitasDoctor.Add(cita);
                }
            }
        }

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Doctor");
            Application.Current.MainPage = new NavigationPage(new MainPagemenu(userRole));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
