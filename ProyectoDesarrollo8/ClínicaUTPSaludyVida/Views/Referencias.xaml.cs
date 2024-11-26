using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class Referencias : ContentPage
{
    public Referencias()
    {
        InitializeComponent();
        BindingContext = new ReferenciasViewModel();
    }
    private void OnButtonClicked(object sender, EventArgs e)
    {
        FormularioPaciente.IsVisible = true;
    }
    private void OnButtonClicked1(object sender, EventArgs e)
    {
        FormularioPaciente.IsVisible = false;
    }
}