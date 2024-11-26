using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views;

public partial class Inicio : ContentPage
{
    public Inicio()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        var viewModel = new DoctorViewModel();
        await Navigation.PushAsync(new Doctor(viewModel));
    }

    private async void OnLoginButtonClicked1(object sender, EventArgs e)
    {
        var viewModel = new PacientesViewModel();
        await Navigation.PushAsync(new Pacientes());
    }
    private async void OnRegisterNurseButtonClicked(object sender, EventArgs e)
    {
        var viewModel = new NurseViewModel();
        await Navigation.PushAsync(new NurseRegister(viewModel));
    }
    private async void OnRegisterLabButtonClicked1(object sender, EventArgs e)
    {
        var viewModel = new LaboratoristaViewModel();
        await Navigation.PushAsync(new LaboratoristaRegister(viewModel));
    }

}
