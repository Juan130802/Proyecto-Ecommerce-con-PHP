using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private string _email;
        private string _password;

        public LoginViewModel()
        {
            _dbService = new LocalDbService();
            LoginCommand = new Command(async () => await OnLogin());
        }

        public LoginViewModel(LocalDbService dbService)
        {
            _dbService = dbService;
            LoginCommand = new Command(async () => await OnLogin());
        }

        public ICommand LoginCommand { get; }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private async Task OnLogin()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, ingrese usuario y contraseña.", "Aceptar");
                return;
            }

            if (Email == "Admin" && Password == "12345JJJJA")
            {
                Preferences.Set("userRole", "Admin");
                Application.Current.MainPage = new NavigationPage(new MainPagemenu("Admin"));
                return;
            }

            string userType = await ValidateLogin(Email, Password);

            if (userType == "Paciente")
            {
                var paciente = (await _dbService.GetPacienteRegis())
                               .FirstOrDefault(p => p.Email == Email && p.Password == Password);

                if (paciente != null)
                {
                    Preferences.Set("IdPaciente", paciente.Id.ToString());
                    Preferences.Set("LoggedInEmail", Email);
                    Preferences.Set("userRole", userType);
                    Application.Current.MainPage = new NavigationPage(new MainPagemenu(userType));
                }
            }
            else if (userType == "Doctor")
            {
                var doctor = (await _dbService.GetDoctores())
                             .FirstOrDefault(d => d.Email == Email && d.Password == Password);

                if (doctor != null)
                {
                    Preferences.Set("IdDoctor", doctor.Id.ToString()); // Usa "doctor.Id" si "DoctorId" no es el campo correcto
                    Preferences.Set("LoggedInEmail", Email);
                    Preferences.Set("userRole", userType);
                    Application.Current.MainPage = new NavigationPage(new MainPagemenu(userType));
                }
            }


            else if (userType == "Enfermera")
            {
                var enfermera = (await _dbService.GetNurse())
                                .FirstOrDefault(e => e.Email == Email && e.Password == Password);

                if (enfermera != null)
                {
                    Preferences.Set("IdEnfermera", enfermera.Id.ToString());
                    Preferences.Set("LoggedInEmail", Email);
                    Preferences.Set("userRole", userType);
                    Application.Current.MainPage = new NavigationPage(new MainPagemenu(userType));
                }
            }
            else if (userType == "Laboratorista")
            {
                var laboratorista = (await _dbService.GetLabs())
                                    .FirstOrDefault(l => l.Email == Email && l.Password == Password);

                if (laboratorista != null)
                {
                    Preferences.Set("IdLaboratorista", laboratorista.Id.ToString());
                    Preferences.Set("LoggedInEmail", Email);
                    Preferences.Set("userRole", userType);
                    Application.Current.MainPage = new NavigationPage(new MainPagemenu(userType));
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrectos", "Aceptar");
            }

            Email = string.Empty;
            Password = string.Empty;
        }

        private async Task<string> ValidateLogin(string email, string password)
        {
            var pacientes = await _dbService.GetPacienteRegis();
            if (pacientes.Any(u => u.Email == email && u.Password == password))
            {
                return "Paciente";
            }

            var doctores = await _dbService.GetDoctores();
            if (doctores.Any(d => d.Email == email && d.Password == password))
            {
                return "Doctor";
            }

            var enfermeras = await _dbService.GetNurse();
            if (enfermeras.Any(e => e.Email == email && e.Password == password))
            {
                return "Enfermera";
            }

            var laboratoristas = await _dbService.GetLabs();
            if (laboratoristas.Any(l => l.Email == email && l.Password == password))
            {
                return "Laboratorista";
            }

            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
