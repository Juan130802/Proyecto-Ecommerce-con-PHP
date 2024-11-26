using Clínica_UTP_Salud_y_Vida.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class DonantesViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly LocalDbService _localDbService;
        private Timer _timer;
        private string _cedulaBusqueda;
        private string _nombre;
        private string _cedula;
        private string _tipoSangre;
        private DateTime? _fechaDeExamen;
        private string _status;
        private DateTime? _fechaDonacion;
        private int _idDonante;

        public string Nombre
        {
            get => _nombre;
            set { _nombre = value; OnPropertyChanged(nameof(Nombre)); }
        }

        public string Cedula
        {
            get => _cedula;
            set { _cedula = value; OnPropertyChanged(nameof(Cedula)); }
        }

        public string CedulaBusqueda
        {
            get => _cedulaBusqueda;
            set { _cedulaBusqueda = value; OnPropertyChanged(nameof(CedulaBusqueda)); }
        }

        public string TipoSangre
        {
            get => _tipoSangre;
            set { _tipoSangre = value; OnPropertyChanged(nameof(TipoSangre)); }
        }

        public DateTime? FechaDeExamen
        {
            get => _fechaDeExamen;
            set { _fechaDeExamen = value; OnPropertyChanged(nameof(FechaDeExamen)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public DateTime? FechaDonacion
        {
            get => _fechaDonacion;
            set { _fechaDonacion = value; OnPropertyChanged(nameof(FechaDonacion)); }
        }

        public ObservableCollection<Donantes> DonantesEncontrados { get; private set; }
        public ObservableCollection<string> ResultadosExamen { get; private set; }
        public ICommand RealizarExamenCommand { get; }
        public ICommand BuscarDonanteCommand { get; }
        public ICommand AceptarDonacionCommand { get; }
        public ICommand RechazarDonacionCommand { get; }

        public DonantesViewModel()
        {
            _localDbService = new LocalDbService();
            DonantesEncontrados = new ObservableCollection<Donantes>();
            ResultadosExamen = new ObservableCollection<string>();
            RealizarExamenCommand = new Command(async () => await RealizarExamen());
            BuscarDonanteCommand = new Command(async () => await BuscarDonante());
            AceptarDonacionCommand = new Command(async () => await AceptarDonacion());
            RechazarDonacionCommand = new Command(async () => await RechazarDonacion());

            _timer = new Timer(120000);
            _timer.Elapsed += CheckStatusUpdate;
            _timer.Start();
        }
        private async Task RealizarExamen()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Cedula))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese el nombre y la cédula del paciente.", "Aceptar");
                return;
            }

            var donante = await _localDbService.ObtenerDonantePorCedula(Cedula);
            if (donante == null)
            {
                donante = new Donantes
                {
                    Nombre = Nombre,
                    Cedula = Cedula,
                    TipoSangre = GenerarTipoSangreAleatorio(),
                    FechaDeExamen = DateTime.Now,
                    Status = "En Espera"
                };
                await _localDbService.Insert(donante);
                _idDonante = donante.Id;
            }
            else
            {
                donante.FechaDeExamen = DateTime.Now;
                donante.Status = "En Espera";
                if (string.IsNullOrEmpty(donante.TipoSangre))
                {
                    donante.TipoSangre = GenerarTipoSangreAleatorio();
                }
                _idDonante = donante.Id;
            }

            await _localDbService.Update(donante);
            ResultadosExamen.Add($"Examen de {Nombre} realizado el {donante.FechaDeExamen:dd/MM/yyyy}, tipo de sangre: {donante.TipoSangre}");
            Nombre = string.Empty;
            Cedula = string.Empty;
        }
        private async Task BuscarDonante()
        {
            if (string.IsNullOrWhiteSpace(CedulaBusqueda))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese una cédula para buscar.", "Aceptar");
                return;
            }

            var donante = await _localDbService.ObtenerDonantePorCedula(CedulaBusqueda);
            if (donante != null)
            {
                Nombre = donante.Nombre;
                Cedula = donante.Cedula;
                TipoSangre = donante.TipoSangre;
                FechaDeExamen = donante.FechaDeExamen;
                Status = donante.Status;
                FechaDonacion = donante.Fecha_Donacion;
                _idDonante = donante.Id;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Aviso", "No se encontró ningún donante con esa cédula.", "Aceptar");
                Nombre = string.Empty;
                Cedula = string.Empty;
                TipoSangre = string.Empty;
                FechaDeExamen = null;
                Status = "En Espera";
                FechaDonacion = null;
            }
        }
        private async Task AceptarDonacion()
        {
            if (_idDonante > 0)
            {
                var donante = await _localDbService.ObtenerDonantePorId(_idDonante);
                if (donante != null)
                {
                    if (donante.Status == "Positivo")
                    {
                        donante.Fecha_Donacion = DateTime.Now;
                        await _localDbService.Update(donante);
                        FechaDonacion = donante.Fecha_Donacion;
                        await App.Current.MainPage.DisplayAlert("Éxito", "Fecha de donación guardada con éxito.", "Aceptar");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "Sus exámenes salieron negativos, no puede donar.", "Aceptar");
                    }
                }
            }
        }
        private async Task RechazarDonacion()
        {
            await App.Current.MainPage.DisplayAlert("Gracias", "Gracias por su tiempo.", "Aceptar");
        }

        private async void CheckStatusUpdate(object sender, ElapsedEventArgs e)
        {
            var donantesEnEspera = await _localDbService.ObtenerDonantesConEstadoEnEspera();
            foreach (var donante in donantesEnEspera)
            {
                var tiempoTranscurrido = DateTime.Now - donante.FechaDeExamen;
                if (tiempoTranscurrido.TotalMinutes >= 2)
                {
                    donante.Status = new Random().Next(0, 2) == 0 ? "Positivo" : "Negativo";
                    await _localDbService.Update(donante);
                    if (CedulaBusqueda == donante.Cedula)
                    {
                        Status = donante.Status;
                    }
                }
            }
        }
        private string GenerarTipoSangreAleatorio()
        {
            var tiposSangre = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            return tiposSangre[new Random().Next(tiposSangre.Length)];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
