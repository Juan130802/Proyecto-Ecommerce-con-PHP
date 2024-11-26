using Clínica_UTP_Salud_y_Vida.ViewModels;

namespace Clínica_UTP_Salud_y_Vida.Views
{
    public partial class ReferenciasListPage : ContentPage
    {
        private ReferenciasListViewModel viewModel;

        public ReferenciasListPage()
        {
            InitializeComponent();
            viewModel = (ReferenciasListViewModel)BindingContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.CargarReferencias();
        }
    }
}
