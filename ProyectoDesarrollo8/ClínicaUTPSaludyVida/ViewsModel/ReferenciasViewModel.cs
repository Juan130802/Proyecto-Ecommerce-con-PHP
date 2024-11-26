using Clínica_UTP_Salud_y_Vida.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class ReferenciasViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private string _name;
        private string _cedula;
        private string _contacto;
        private string _motivo;
        private string _especialidadMedica;
        private string _editReferenciaId;
        private PacientesRegister _pacienteSeleccionado;

        public ReferenciasViewModel()
        {
            _dbService = new LocalDbService();
            SaveCommand = new Command(OnSave);
            SaveCommand1 = new Command(OnSave1);
            BuscarCommand = new Command<string>(async (cedula) => await BuscarPacientesPorCedula(cedula));
            SeleccionarPacienteCommand = new Command<PacientesRegister>(OnPacienteSeleccionado);
        }

        public ICommand SaveCommand { get; }
        public ICommand SaveCommand1 { get; }
        public ICommand BuscarCommand { get; }
        public ICommand SeleccionarPacienteCommand { get; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public string Contacto
        {
            get => _contacto;
            set
            {
                _contacto = value;
                OnPropertyChanged(nameof(Contacto));
            }
        }

        public string Motivo
        {
            get => _motivo;
            set
            {
                _motivo = value;
                OnPropertyChanged(nameof(Motivo));
            }
        }

        public string EspecialidadMedica
        {
            get => _especialidadMedica;
            set
            {
                _especialidadMedica = value;
                OnPropertyChanged(nameof(EspecialidadMedica));
            }
        }

        public PacientesRegister PacienteSeleccionado
        {
            get => _pacienteSeleccionado;
            set
            {
                _pacienteSeleccionado = value;
                OnPacienteSeleccionado(value);
                OnPropertyChanged(nameof(PacienteSeleccionado));
            }
        }

        private ObservableCollection<PacientesRegister> _listaPacientes = new ObservableCollection<PacientesRegister>();
        private int _editPacienteId;

        public ObservableCollection<PacientesRegister> ListaPacientes
        {
            get => _listaPacientes;
            set
            {
                _listaPacientes = value;
                OnPropertyChanged(nameof(ListaPacientes));
            }
        }

        private void OnPacienteSeleccionado(PacientesRegister paciente)
        {
            if (paciente != null)
            {
                Name = paciente.Name;
                Cedula = paciente.Cedula;
            }
        }


        private async void OnSave1()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Cedula))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos están vacíos. Por favor, ingrese los valores.", "Aceptar");
                return;
            }

            if (_editPacienteId == 0)
            {
                await _dbService.Create(new PacientesRegister
                {
                    Name = Name,
                    Cedula = Cedula,
                });

                await Application.Current.MainPage.DisplayAlert("Éxito", "Paciente registrado correctamente.", "Aceptar");
            }
        }

        private async void OnSave()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Cedula) || string.IsNullOrWhiteSpace(Motivo) || string.IsNullOrWhiteSpace(EspecialidadMedica))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, complete todos los campos requeridos.", "Aceptar");
                return;
            }

            var nuevaReferencia = new Referencia
            {
                Name = Name,
                Cedula = Cedula,
                Contacto = Contacto,
                Motivo = Motivo,
                especialidadmedica = EspecialidadMedica
            };

            int idReferencia = await _dbService.CreateReferencia(nuevaReferencia);

            if (idReferencia <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo registrar la referencia.", "Aceptar");
                return;
            }

            string rutaPdf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Referencia_{Name}.pdf");

            try
            {
                await _dbService.GenerarPdfReferenciaMedica(idReferencia, rutaPdf);

                await Application.Current.MainPage.DisplayAlert("Éxito", "PDF generado correctamente.", "Aceptar");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Hubo un problema al generar el PDF: {ex.Message}", "Aceptar");
            }
        }

        public async Task BuscarPacientesPorCedula(string cedula)
        {
            var pacientes = await _dbService.BuscarPorCedula(cedula);
            ListaPacientes = new ObservableCollection<PacientesRegister>(pacientes);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
