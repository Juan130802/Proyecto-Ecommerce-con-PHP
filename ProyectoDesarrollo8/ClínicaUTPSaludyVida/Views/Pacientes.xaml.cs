using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class Pacientes : ContentPage
{
    public Pacientes(PacientesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    public Pacientes() : this(new PacientesViewModel())
    {
    }
}
