using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class CertificadoPage : ContentPage
{
    public CertificadoPage()
    {
        InitializeComponent();
        BindingContext = new CertificadosViewModel();
    }
}