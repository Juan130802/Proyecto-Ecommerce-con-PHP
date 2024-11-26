using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class CitasAgendadasUsuario : ContentPage
    {
        public CitasAgendadasUsuario(LocalDbService localDbService)
        {
            InitializeComponent();
            BindingContext = new CitasAgendadasUsuarioViewModel(new LocalDbService());
        }
    }
}
