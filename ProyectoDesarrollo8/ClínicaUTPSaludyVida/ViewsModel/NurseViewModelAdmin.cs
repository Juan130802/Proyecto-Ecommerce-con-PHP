using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.Views;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel
{
    public partial class NurseViewModelAdmin : ObservableObject
    {
        private readonly LocalDbService _localDbService;
        public ObservableCollection<Nurse> ListaEnfermeras { get; private set; }

        public ICommand VerCitasCommand { get; }
        public ICommand EnfermeraTappedCommand { get; }

        public NurseViewModelAdmin()
        {
            _localDbService = new LocalDbService();
            ListaEnfermeras = new ObservableCollection<Nurse>();
            _ = LoadNursesAsync();
            EnfermeraTappedCommand = new Command<Nurse>(async (nurse) => await MostrarOpciones(nurse));
        }

        private async Task LoadNursesAsync()
        {
            var nurses = await _localDbService.GetNurse();
            foreach (var nurse in nurses)
            {
                ListaEnfermeras.Add(nurse);
            }
        }

        private async Task MostrarOpciones(Nurse nurse)
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Acción", "Cancelar", null, "Eliminar", "Actualizar");

            if (action == "Eliminar")
            {
                bool confirmacion = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Deseas eliminar esta enfermera?", "Sí", "No");
                if (confirmacion)
                {
                    await _localDbService.Delete(nurse);
                    ListaEnfermeras.Remove(nurse);
                }
            }
            else if (action == "Actualizar")
            {
                var editarNursePage = new NurseRegister(new NurseViewModel(nurse));
                await Application.Current.MainPage.Navigation.PushAsync(editarNursePage);
            }
        }
    }
}
