using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class ConsultaGeneral : ContentPage
    {
        public ConsultaGeneral()
        {
            InitializeComponent();
            BindingContext = new PacientesViewModel();
        }
        private async void OnPacienteTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is PacientesRegister pacienteSeleccionado)
            {
                await Navigation.PushAsync(new FormularioHistorialPage(pacienteSeleccionado.Id, pacienteSeleccionado.Name, pacienteSeleccionado.Cedula));
            }
        }

    }
}
