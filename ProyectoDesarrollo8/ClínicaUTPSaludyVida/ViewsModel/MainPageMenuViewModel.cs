using Clínica_UTP_Salud_y_Vida.Admin.Views;
using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public class MainPageMenuViewModel : INotifyPropertyChanged
    {
        private readonly string _userRole;

        public MainPageMenuViewModel()
            : this("Paciente")
        {
        }

        public MainPageMenuViewModel(string userRole)
        {
            _userRole = userRole;
            ShowRelevantMenu();
            ToggleMenuCommand = new Command(OnMenuButtonClicked);
            MenuOptionCommand = new Command<string>(OnMenuOptionClicked);
            LogoutCommand = new Command(OnLogout);
        }

        public ICommand ToggleMenuCommand { get; }
        public ICommand MenuOptionCommand { get; }
        public ICommand LogoutCommand { get; }

        private bool _isPatientMenuVisible;
        public bool IsPatientMenuVisible
        {
            get => _isPatientMenuVisible;
            set
            {
                _isPatientMenuVisible = value;
                OnPropertyChanged(nameof(IsPatientMenuVisible));
            }
        }

        private bool _isDoctorMenuVisible;
        public bool IsDoctorMenuVisible
        {
            get => _isDoctorMenuVisible;
            set
            {
                _isDoctorMenuVisible = value;
                OnPropertyChanged(nameof(IsDoctorMenuVisible));
            }
        }

        private bool _isAdminMenuVisible;
        public bool IsAdminMenuVisible
        {
            get => _isAdminMenuVisible;
            set
            {
                _isAdminMenuVisible = value;
                OnPropertyChanged(nameof(IsAdminMenuVisible));
            }
        }

        private bool _isNurseMenuVisible;
        public bool IsNurseMenuVisible
        {
            get => _isNurseMenuVisible;
            set
            {
                _isNurseMenuVisible = value;
                OnPropertyChanged(nameof(IsNurseMenuVisible));
            }
        }

        private bool _isLaboratoristaMenuVisible;
        public bool IsLaboratoristaMenuVisible
        {
            get => _isLaboratoristaMenuVisible;
            set
            {
                _isLaboratoristaMenuVisible = value;
                OnPropertyChanged(nameof(IsLaboratoristaMenuVisible));
            }
        }

        private void ShowRelevantMenu()
        {
            IsPatientMenuVisible = _userRole == "Paciente";
            IsDoctorMenuVisible = _userRole == "Doctor";
            IsAdminMenuVisible = _userRole == "Admin";
            IsNurseMenuVisible = _userRole == "Enfermera";
            IsLaboratoristaMenuVisible = _userRole == "Laboratorista";

        }

        private void OnMenuButtonClicked()
        {
            if (_userRole == "Paciente")
                IsPatientMenuVisible = !IsPatientMenuVisible;
            else if (_userRole == "Doctor")
                IsDoctorMenuVisible = !IsDoctorMenuVisible;
            else if (_userRole == "Admin")
                IsAdminMenuVisible = !IsAdminMenuVisible;
            else if (_userRole == "Enfermera")
                IsNurseMenuVisible = !IsNurseMenuVisible;
            else if (_userRole == "Laboratorista")
                IsLaboratoristaMenuVisible = !IsLaboratoristaMenuVisible;
        }

        private async void OnMenuOptionClicked(string option)
        {
            switch (option)
            {
                case "Agende su cita":
                    await Application.Current.MainPage.Navigation.PushAsync(new Usuarios2(new LocalDbService()));
                    break;
                case "Ver Cita Agendada":
                    await Application.Current.MainPage.Navigation.PushAsync(new CitasAgendadasUsuario(new LocalDbService()));
                    break;
                case "Recetas Agendadas":
                    await Application.Current.MainPage.Navigation.PushAsync(new RecetasAgendadas(new RecetasAsignadasViewModel()));
                    break;
                case "Historial Medico":
                    await Application.Current.MainPage.Navigation.PushAsync(new HistorialMedicoPacientes());
                    break;
                case "Servicios Ofertados":
                    await Application.Current.MainPage.Navigation.PushAsync(new ServiciosOfertados());
                    break;
                case "Nosotros":
                    await Application.Current.MainPage.Navigation.PushAsync(new Contacto());
                    break;
                case "Registro de Citas":
                    await Application.Current.MainPage.Navigation.PushAsync(new CitasAgendadasDoctor(new LocalDbService()));
                    break;
                case "Calendario de Citas":
                    await Application.Current.MainPage.Navigation.PushAsync(new CalendarioDeCitas());
                    break;
                case "Referencias Medicas":
                    await Application.Current.MainPage.Navigation.PushAsync(new Referencias());
                    break;
                case "Contacto":
                    await Application.Current.MainPage.Navigation.PushAsync(new Contacto());
                    break;
                case "Registro":
                    await Application.Current.MainPage.Navigation.PushAsync(new ReferenciasListPage());
                    break;
                case "Doctores":
                    await Application.Current.MainPage.Navigation.PushAsync(new Admin.Views.Doctores());
                    break;
                case "Pacientes":
                    await Application.Current.MainPage.Navigation.PushAsync(new Pacientes1());
                    break;
                case "Personal Médico":
                    await Application.Current.MainPage.Navigation.PushAsync(new PersonalMedico());
                    break;
                case "ProcedimientosMédico":
                    await Application.Current.MainPage.Navigation.PushAsync(new AdminProcedimientosPage(new AdminProcedimientosViewModel()));
                    break;
                case "Consulta General":
                    await Application.Current.MainPage.Navigation.PushAsync(new ConsultaGeneral());
                    break;
                case "Consulta Urgencias":
                    await Application.Current.MainPage.Navigation.PushAsync(new ConsultaUrgencias());
                    break;
                case "Medicamentos Tratamientos":
                    await Application.Current.MainPage.Navigation.PushAsync(new MedicamentosTratamientos());
                    break;
                case "Certificados Medicos":
                    await Application.Current.MainPage.Navigation.PushAsync(new CertificadoPage());
                    break;
                case "Donantes De Sangre":
                    await Application.Current.MainPage.Navigation.PushAsync(new DonadoresDeSangre(new DonantesViewModel()));
                    break;
                case "ExamenesDeDonantes":
                    await Application.Current.MainPage.Navigation.PushAsync(new ExamenesDeDonantesPage(new DonantesViewModel()));
                    break;
                case "Procedimientos Medicos":
                    await Application.Current.MainPage.Navigation.PushAsync(new ProcedimientoMedicoPage(new ProcedimientoMedicoViewModel()));
                    break;
                default:
                    await Application.Current.MainPage.DisplayAlert("Error", "Opción no reconocida", "OK");
                    break;
            }
            if (_userRole == "Paciente")
                IsPatientMenuVisible = false;
            else if (_userRole == "Doctor")
                IsDoctorMenuVisible = false;
            else if (_userRole == "Admin")
                IsAdminMenuVisible = false;
            else if (_userRole == "Enfermera")
                IsNurseMenuVisible = false;
            else if (_userRole == "Laboratorista")
                IsLaboratoristaMenuVisible = false;
        }

        private void OnLogout()
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
