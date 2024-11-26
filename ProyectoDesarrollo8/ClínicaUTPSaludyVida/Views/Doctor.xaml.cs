using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class Doctor : ContentPage
{

    public Doctor(DoctorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}