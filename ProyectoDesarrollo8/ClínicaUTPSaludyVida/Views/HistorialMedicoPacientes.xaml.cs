using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class HistorialMedicoPacientes : ContentPage
    {
        public HistorialMedicoPacientes()
        {
            InitializeComponent();
            BindingContext = new HistorialMedicoPacientesViewModel();
        }
    }
}
