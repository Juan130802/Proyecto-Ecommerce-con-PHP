using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class LaboratoristaViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;
        private string _name;
        private string _cedula;
        private string _email;
        private string _password;
        private int _editLabId;

        public LaboratoristaViewModel()
        {
            _dbService = new LocalDbService();
            SaveCommand = new Command(OnSave);
            LoginCommand = new Command(OnLogin);
            LoadExamsCommand = new Command(async () => await LoadExams());
            Examenes = new ObservableCollection<Donantes>();
        }

        public LaboratoristaViewModel(Laboratorista laboratorista) : this()
        {
            if (laboratorista != null)
            {
                _editLabId = laboratorista.Id;
                Name = laboratorista.Name;
                Cedula = laboratorista.Cedula;
                Email = laboratorista.Email;
                Password = laboratorista.Password;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LoadExamsCommand { get; }

        public ObservableCollection<Donantes> Examenes { get; set; }

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

            var existingLab = (await _dbService.GetLabs()).FirstOrDefault(l => l.Email == Email || l.Cedula == Cedula);

            if (_editLabId == 0 && existingLab != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ya existe un laboratorista con este email o cédula.", "Aceptar");
                return;
            }

            if (_editLabId != 0 && existingLab != null && existingLab.Id != _editLabId)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Ya existe un laboratorista con este email o cédula.", "Aceptar");
                return;
            }

            var lab = new Laboratorista
            {
                Id = _editLabId,
                Name = Name,
                Cedula = Cedula,
                Email = Email,
                Password = Password
            };

            if (_editLabId == 0)
            {
                await _dbService.CreateLab(lab);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Laboratorista registrada correctamente.", "Aceptar");
            }
            else
            {
                await _dbService.UpdateLab(lab);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Laboratorista actualizada correctamente.", "Aceptar");
                await IrInicio();
            }

            Name = string.Empty;
            Cedula = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            _editLabId = 0;
        }

        private async Task LoadExams()
        {
            Examenes.Clear();
            var donantes = await _dbService.GetDonantes();
            foreach (var donantes1 in donantes)
            {
                Examenes.Add(donantes1);
            }
        }

        private async Task IrInicio()
        {
            string userRole = Preferences.Get("userRole", "Laboratorista");
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
