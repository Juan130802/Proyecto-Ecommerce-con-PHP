using Clínica_UTP_Salud_y_Vida.Models;
using Clínica_UTP_Salud_y_Vida.ViewsModel;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class MainPagemenu : ContentPage
    {
        public MainPagemenu(string userRole)
        {
            InitializeComponent();
            BindingContext = new MainPageMenuViewModel(userRole);
        }

        private void CerrarSesion(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login1(new LocalDbService()));
        }
    }
}
