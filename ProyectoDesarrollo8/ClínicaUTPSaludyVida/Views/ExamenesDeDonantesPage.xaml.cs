using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class ExamenesDeDonantesPage : ContentPage
{
    public ExamenesDeDonantesPage(DonantesViewModel donantesViewModel)
    {
        InitializeComponent();
        BindingContext = donantesViewModel;
    }
}