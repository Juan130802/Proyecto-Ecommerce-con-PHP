using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public partial class DoctoresViewModel : ObservableObject
    {
        private readonly LocalDbService _localDbService;
        public ObservableCollection<Doctores> ListaDoctores { get; private set; }

        public ICommand VerCitasCommand { get; }
        public ICommand DoctorTappedCommand { get; }

        public DoctoresViewModel()
        {
            _localDbService = new LocalDbService();
            ListaDoctores = new ObservableCollection<Doctores>();
            LoadDoctoresAsync();

            VerCitasCommand = new Command<Doctores>(async (doctor) => await VerCitas(doctor));
            DoctorTappedCommand = new Command<Doctores>(async (doctor) => await MostrarOpciones(doctor));
        }

        private async Task LoadDoctoresAsync()
        {
            var doctores = await _localDbService.GetDoctores();
            foreach (var doctor in doctores)
            {
                ListaDoctores.Add(doctor);
            }
        }

        private async Task VerCitas(Doctores doctor)
        {
            Preferences.Set("IdDoctor", doctor.DoctorId.ToString());
            await Application.Current.MainPage.Navigation.PushAsync(new CitasAgendadasDoctor(new LocalDbService()));
        }

        private async Task MostrarOpciones(Doctores doctor)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Acción", "Cancelar", null, "Eliminar", "Actualizar");

            if (action == "Eliminar")
            {
                bool confirmacion = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas eliminar este doctor?", "Sí", "No");
                if (confirmacion)
                {
                    await _localDbService.Delete(doctor);
                    ListaDoctores.Remove(doctor);
                }
            }
            else if (action == "Actualizar")
            {
                var editarDoctorPage = new Doctor(new DoctorViewModel(doctor));
                await Application.Current.MainPage.Navigation.PushAsync(editarDoctorPage);
            }
        }
    }
}
