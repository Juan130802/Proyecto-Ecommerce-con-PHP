using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class NurseRegister : ContentPage
{
    public NurseRegister(NurseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}