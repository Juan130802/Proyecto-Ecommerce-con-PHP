using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class CertificadosViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private string _nombre;
        private string _cedula;
        public event PropertyChangedEventHandler PropertyChanged;
        private int _pacienteId;

        public ICommand GuardarYDescargarCommand { get; }

        public ICommand IrInicioCommand { get; }

        public CertificadosViewModel()
        {
            _dbService = new LocalDbService();
            GuardarYDescargarCommand = new Command(async () => await GuardarYDescargar());
            IrInicioCommand = new Command(async () => await IrInicio());
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

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Paciente");
            Application.Current.MainPage = new NavigationPage(new MainPagemenu(userRole));
        }

        private async Task GuardarYDescargar()
        {
            var customer = new Customer
            {
                Nombre = Nombre,
                Cedula = Cedula
            };

            await _dbService.CreateCustomer(customer);
            await Application.Current.MainPage.DisplayAlert("Éxito", "Certificado guardado correctamente", "OK");

            string ruta = Path.Combine(@"D:\HP\Nueva carpeta\Juan Tareas", $"Certificado_Buena_Salud_De{customer.Nombre}.pdf");

            try
            {
                string resultado = await _dbService.GenerarCertificadoPDF(customer.PacienteId, ruta, customer.Id);

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


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
