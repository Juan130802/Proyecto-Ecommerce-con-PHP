using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class LaboratoristaRegister : ContentPage
{
    public LaboratoristaRegister(LaboratoristaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}