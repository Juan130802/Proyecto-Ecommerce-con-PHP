using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class CitasAgendadasDoctor : ContentPage
    {
        private readonly LocalDbService _localDbService;

        public CitasAgendadasDoctor(LocalDbService localDbService)
        {
            InitializeComponent();
            _localDbService = localDbService;
            BindingContext = new CitasAgendadasDoctorViewModel();
        }

        private async void NavegarAReceta(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecetaPage(_localDbService));
        }
    }
}
