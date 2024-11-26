using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Admin.Views;

public partial class Doctores : ContentPage
{
    public Doctores()
    {
        InitializeComponent();
        BindingContext = new DoctoresViewModel();
    }
}