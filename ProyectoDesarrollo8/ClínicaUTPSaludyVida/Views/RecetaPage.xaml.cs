using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class RecetaPage : ContentPage
{
    public RecetaPage(LocalDbService localDbService)
    {
        InitializeComponent();
        BindingContext = new RecetaViewModel(localDbService);
    }
}