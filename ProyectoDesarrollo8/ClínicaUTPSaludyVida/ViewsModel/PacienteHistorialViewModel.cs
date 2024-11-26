using Clínica_UTP_Salud_y_Vida.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class PacienteHistorialViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private int _pacienteId;
        private bool _esActualizar;
        private string _nombre;
        private string _cedula;
        private string _telefono;
        private string _edad;
        private float _temperaturaCorporal;
        private int _frecuenciaCardiaca;
        private int _frecuenciaRespiratoria;
        private string _presionArterial;
        private int _oxigeno;
        private float _peso;
        private float _altura;
        private string _motivoConsulta;
        private string _enfermedadesPrevias;
        private string _medicamentosActuales;
        private string _alergiasConocidas;
        private int _nivelDolor;

        public event PropertyChangedEventHandler PropertyChanged;

        public PacienteHistorialViewModel(int pacienteId, string nombre, string cedula, bool esActualizar = false)
        {
            _dbService = new LocalDbService();
            _pacienteId = pacienteId;
            Nombre = nombre;
            Cedula = cedula;
            _esActualizar = esActualizar;
            GuardarCommand = new Command(OnGuardar);

            if (_esActualizar)
            {
                CargarDatosHistorial();
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public string Cedula
        {
            get => _cedula;
            set
            {
                _cedula = value;
                OnPropertyChanged(nameof(Cedula));
            }
        }

        public string Telefono
        {
            get => _telefono;
            set
            {
                _telefono = value;
                OnPropertyChanged(nameof(Telefono));
            }
        }

        public string Edad
        {
            get => _edad;
            set
            {
                _edad = value;
                OnPropertyChanged(nameof(Edad));
            }
        }

        public float TemperaturaCorporal
        {
            get => _temperaturaCorporal;
            set
            {
                _temperaturaCorporal = value;
                OnPropertyChanged(nameof(TemperaturaCorporal));
            }
        }

        public int FrecuenciaCardiaca
        {
            get => _frecuenciaCardiaca;
            set
            {
                _frecuenciaCardiaca = value;
                OnPropertyChanged(nameof(FrecuenciaCardiaca));
            }
        }

        public int FrecuenciaRespiratoria
        {
            get => _frecuenciaRespiratoria;
            set
            {
                _frecuenciaRespiratoria = value;
                OnPropertyChanged(nameof(FrecuenciaRespiratoria));
            }
        }

        public string PresionArterial
        {
            get => _presionArterial;
            set
            {
                _presionArterial = value;
                OnPropertyChanged(nameof(PresionArterial));
            }
        }

        public int Oxigeno
        {
            get => _oxigeno;
            set
            {
                _oxigeno = value;
                OnPropertyChanged(nameof(Oxigeno));
            }
        }

        public float Peso
        {
            get => _peso;
            set
            {
                _peso = value;
                OnPropertyChanged(nameof(Peso));
            }
        }

        public float Altura
        {
            get => _altura;
            set
            {
                _altura = value;
                OnPropertyChanged(nameof(Altura));
            }
        }

        public string MotivoConsulta
        {
            get => _motivoConsulta;
            set
            {
                _motivoConsulta = value;
                OnPropertyChanged(nameof(MotivoConsulta));
            }
        }

        public string EnfermedadesPrevias
        {
            get => _enfermedadesPrevias;
            set
            {
                _enfermedadesPrevias = value;
                OnPropertyChanged(nameof(EnfermedadesPrevias));
            }
        }

        public string MedicamentosActuales
        {
            get => _medicamentosActuales;
            set
            {
                _medicamentosActuales = value;
                OnPropertyChanged(nameof(MedicamentosActuales));
            }
        }

        public string AlergiasConocidas
        {
            get => _alergiasConocidas;
            set
            {
                _alergiasConocidas = value;
                OnPropertyChanged(nameof(AlergiasConocidas));
            }
        }

        public int NivelDolor
        {
            get => _nivelDolor;
            set
            {
                _nivelDolor = value;
                OnPropertyChanged(nameof(NivelDolor));
            }
        }

        public ICommand GuardarCommand { get; }

        private async void OnGuardar()
        {
            var historial = new PacienteHistorial
            {
                PacienteId = _pacienteId,
                Nombre = Nombre,
                Cedula = Cedula,
                Edad = Edad,
                Telefono = Telefono,
                TemperaturaCorporal = TemperaturaCorporal,
                FrecuenciaCardiaca = FrecuenciaCardiaca,
                FrecuenciaRespiratoria = FrecuenciaRespiratoria,
                PresionArterial = PresionArterial,
                Oxigeno = Oxigeno,
                Peso = Peso,
                Altura = Altura,
                MotivoConsulta = MotivoConsulta,
                EnfermedadesPrevias = EnfermedadesPrevias,
                MedicamentosActuales = MedicamentosActuales,
                AlergiasConocidas = AlergiasConocidas,
                NivelDolor = NivelDolor
            };

            if (_esActualizar)
            {
                await _dbService.ActualizarHistorialMedico(historial);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Historial médico actualizado correctamente.", "Aceptar");
            }
            else
            {
                await _dbService.GuardarHistorialMedico(historial);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Historial médico guardado correctamente.", "Aceptar");
            }

            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            Nombre = string.Empty;
            Cedula = string.Empty;
            Telefono = string.Empty;
            Edad = string.Empty;
            TemperaturaCorporal = 0;
            FrecuenciaCardiaca = 0;
            FrecuenciaRespiratoria = 0;
            PresionArterial = string.Empty;
            Oxigeno = 0;
            Peso = 0;
            Altura = 0;
            MotivoConsulta = string.Empty;
            EnfermedadesPrevias = string.Empty;
            MedicamentosActuales = string.Empty;
            AlergiasConocidas = string.Empty;
            NivelDolor = 0;
        }

        private async void CargarDatosHistorial()
        {
            var historial = await _dbService.ObtenerHistorialPorId(_pacienteId);
            if (historial != null)
            {
                Nombre = historial.Nombre;
                Cedula = historial.Cedula;
                Telefono = historial.Telefono;
                Edad = historial.Edad;
                TemperaturaCorporal = historial.TemperaturaCorporal;
                FrecuenciaCardiaca = historial.FrecuenciaCardiaca;
                FrecuenciaRespiratoria = historial.FrecuenciaRespiratoria;
                PresionArterial = historial.PresionArterial;
                Oxigeno = historial.Oxigeno;
                Peso = historial.Peso;
                Altura = historial.Altura;
                MotivoConsulta = historial.MotivoConsulta;
                EnfermedadesPrevias = historial.EnfermedadesPrevias;
                MedicamentosActuales = historial.MedicamentosActuales;
                AlergiasConocidas = historial.AlergiasConocidas;
                NivelDolor = historial.NivelDolor;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
