using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class FormularioHistorialPage : ContentPage
    {
        public FormularioHistorialPage(int pacienteId, string nombre, string cedula)
        {
            InitializeComponent();
            BindingContext = new PacienteHistorialViewModel(pacienteId, nombre, cedula);
        }
    }
}
