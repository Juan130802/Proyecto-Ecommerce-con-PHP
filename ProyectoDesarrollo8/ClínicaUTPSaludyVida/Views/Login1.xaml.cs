using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class Login1 : ContentPage
    {
        public Login1(LocalDbService dbService)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(dbService);
        }
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Inicio());
        }
    }
}
