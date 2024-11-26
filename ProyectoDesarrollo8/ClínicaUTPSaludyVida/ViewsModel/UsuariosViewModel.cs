using Clínica_UTP_Salud_y_Vida.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Clinica_UTP_Salud_y_Vida.ViewsModel
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;

        public ObservableCollection<Usuarios> UsuariosCollection { get; set; } = new ObservableCollection<Usuarios>();

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _telefono;
        public string Telefono
        {
            get => _telefono;
            set
            {
                _telefono = value;
                OnPropertyChanged();
            }
        }

        private string _tipoConsulta;
        public string TipoConsulta
        {
            get => _tipoConsulta;
            set
            {
                _tipoConsulta = value;
                OnPropertyChanged();
            }
        }

        private DateTime _fechaConsulta;
        public DateTime FechaConsulta
        {
            get => _fechaConsulta;
            set
            {
                _fechaConsulta = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _horaConsulta;
        public TimeSpan HoraConsulta
        {
            get => _horaConsulta;
            set
            {
                _horaConsulta = value;
                OnPropertyChanged();
            }
        }

        private string _motivo;
        public string Motivo
        {
            get => _motivo;
            set
            {
                _motivo = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        public UsuariosViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            SaveCommand = new Command(async () => await SaveCitaAsync());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SaveCitaAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(TipoConsulta) || string.IsNullOrWhiteSpace(Motivo))
            {
                return;
            }

            var citaExistente = await _dbService.GetCitaPorHoraYTipoConsulta(FechaConsulta, HoraConsulta, TipoConsulta);
            if (citaExistente != null)
            {
                return;
            }

            string idPacienteStr = Preferences.Get("IdPaciente", "0");
            if (!int.TryParse(idPacienteStr, out int idPaciente))
            {
                return;
            }

            string doctorIdStr = Preferences.Get("IdDoctor", "0");
            if (!int.TryParse(doctorIdStr, out int doctorId))
            {
                return;
            }

            var usuario = new Usuarios
            {
                name = Name,
                telefono = Telefono,
                tipo_consulta = TipoConsulta,
                Fecha = FechaConsulta,
                Hora = HoraConsulta,
                motivo = Motivo,
                IdPaciente = idPaciente,
                DoctorId = doctorId
            };

            await _dbService.Create(usuario);
        }
    }
}
