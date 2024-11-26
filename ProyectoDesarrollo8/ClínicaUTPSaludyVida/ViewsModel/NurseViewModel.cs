using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class NurseViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private string _name;
        private string _cedula;
        private string _email;
        private string _password;
        private int _editNurseId;

        public NurseViewModel()
        {
            _dbService = new LocalDbService();
            SaveCommand = new Command(OnSave);
            LoginCommand = new Command(OnLogin);
        }

        public NurseViewModel(Nurse nurse)
        {
            _dbService = new LocalDbService();
            SaveCommand = new Command(OnSave);
            LoginCommand = new Command(OnLogin);

            if (nurse != null)
            {
                _editNurseId = nurse.Id;
                Name = nurse.Name;
                Cedula = nurse.Cedula;
                Email = nurse.Email;
                Password = nurse.Password;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoginCommand { get; }

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
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Cedula) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos están vacíos. Por favor, ingrese los valores.", "Aceptar");
                return;
            }

            if (!EsCorreoValido(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El formato del correo no es válido.", "Aceptar");
                return;
            }

            if (_editNurseId == 0)
            {
                await _dbService.Create(new Nurse
                {
                    Name = Name,
                    Cedula = Cedula,
                    Email = Email,
                    Password = Password
                });

                await Application.Current.MainPage.DisplayAlert("Éxito", "Enfermera registrada correctamente.", "Aceptar");
            }
            else
            {
                await _dbService.Update(new Nurse
                {
                    Id = _editNurseId,
                    Name = Name,
                    Cedula = Cedula,
                    Email = Email,
                    Password = Password
                });

                await Application.Current.MainPage.DisplayAlert("Éxito", "Enfermera actualizada correctamente.", "Aceptar");
                await IrInicio();
            }

            Name = string.Empty;
            Cedula = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            _editNurseId = 0;
        }

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Enfermera");
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
