using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class ProcedimientoMedicoPage : ContentPage
{
    public ProcedimientoMedicoPage(ProcedimientoMedicoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}