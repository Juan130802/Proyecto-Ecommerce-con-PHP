using Clínica_UTP_Salud_y_Vida.ViewsModel;


namespace Clínica_UTP_Salud_y_Vida.Admin.Views
{
    public partial class Pacientes1 : ContentPage
    {

        public Pacientes1()
        {
            InitializeComponent();
            BindingContext = new Pacientes1ViewModel();
        }
    }
}
