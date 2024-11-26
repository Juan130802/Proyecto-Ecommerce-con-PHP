using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class DoctorViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private string _numeroDeIdentidad;
        private string _name;
        private string _email;
        private string _password;
        private int _editDoctorId;

        public DoctorViewModel()
        {
            _dbService = new LocalDbService();
            SaveCommand = new Command(OnSave);
            LoginCommand = new Command(OnLogin);
        }

        public DoctorViewModel(Doctores doctor)
        {
            _dbService = new LocalDbService();
            SaveCommand = new Command(OnSave);
            LoginCommand = new Command(OnLogin);

            if (doctor != null)
            {
                _editDoctorId = doctor.Id;
                NumeroDeIdentidad = doctor.NumeroDeIdentidad;
                Name = doctor.Name;
                Email = doctor.Email;
                Password = doctor.Password;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoginCommand { get; }

        public string NumeroDeIdentidad
        {
            get => _numeroDeIdentidad;
            set
            {
                _numeroDeIdentidad = value;
                OnPropertyChanged(nameof(NumeroDeIdentidad));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

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

        private bool EsCorreoValido(string correo)
        {
            var regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, regex);
        }

        private async void OnSave()
        {
            if (string.IsNullOrWhiteSpace(NumeroDeIdentidad) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos son requeridos.", "Aceptar");
                return;
            }

            if (!EsCorreoValido(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El formato del correo no es válido.", "Aceptar");
                return;
            }

            if (_editDoctorId == 0)
            {
                await _dbService.Create(new Doctores
                {
                    NumeroDeIdentidad = NumeroDeIdentidad,
                    Name = Name,
                    Email = Email,
                    Password = Password
                });

                await Application.Current.MainPage.DisplayAlert("Éxito", "Registrado correctamente.", "Aceptar");
            }
            else
            {
                await _dbService.Update(new Doctores
                {
                    Id = _editDoctorId,
                    NumeroDeIdentidad = NumeroDeIdentidad,
                    Name = Name,
                    Email = Email,
                    Password = Password
                });

                await Application.Current.MainPage.DisplayAlert("Éxito", "Actualizado correctamente.", "Aceptar");
                await IrInicio();
            }

            // Limpiar campos
            NumeroDeIdentidad = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            _editDoctorId = 0; // Resetear ID de edición
        }

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Doctor"); // Cambiar a "Doctor" si es necesario
            Application.Current.MainPage = new NavigationPage(new MainPagemenu(userRole));
        }

        private void OnLogin()
        {
            Application.Current.MainPage = new NavigationPage(new Login1(new LocalDbService()));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
