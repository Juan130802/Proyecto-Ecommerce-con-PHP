using Clínica_UTP_Salud_y_Vida.Models;
namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class Bienvenida : ContentPage
{
    public Bienvenida()
    {
        InitializeComponent();
        NavigateToLoginPage();
    }
    private async void NavigateToLoginPage()
    {
        await Task.Delay(3000);

        Application.Current.MainPage = new NavigationPage(new Login1(new LocalDbService()));
    }
}