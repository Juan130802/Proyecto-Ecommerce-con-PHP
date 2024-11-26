using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class DonadoresDeSangre : ContentPage
    {
        public DonadoresDeSangre(DonantesViewModel donantesViewModel)
        {
            InitializeComponent();
            BindingContext = donantesViewModel;
        }
        private void OnButtonClicked(object sender, EventArgs e)
        {
            FormularioDonante.IsVisible = true;
        }
        private void OnButtonClicked1(object sender, EventArgs e)
        {
            FormularioDonante.IsVisible = false;
        }
    }
}
