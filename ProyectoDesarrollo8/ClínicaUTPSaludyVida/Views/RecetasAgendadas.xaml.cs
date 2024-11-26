using Clínica_UTP_Salud_y_Vida.ViewsModel;
namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class RecetasAgendadas : ContentPage
{
    public RecetasAgendadas(RecetasAsignadasViewModel recetasAsignadasViewModel)
    {
        InitializeComponent();
        BindingContext = new RecetasAsignadasViewModel();
    }
}