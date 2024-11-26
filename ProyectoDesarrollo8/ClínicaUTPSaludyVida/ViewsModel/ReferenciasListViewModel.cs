using Clínica_UTP_Salud_y_Vida.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewModels
{
    public class ReferenciasListViewModel : INotifyPropertyChanged
    {
        private readonly LocalDbService _dbService;

        public ObservableCollection<Referencia> Referencias { get; set; }

        public ReferenciasListViewModel()
        {
            _dbService = new LocalDbService();
            Referencias = new ObservableCollection<Referencia>();
            CargarReferenciasCommand = new Command(async () => await CargarReferencias());
        }

        public ICommand CargarReferenciasCommand { get; }

        public async Task CargarReferencias()
        {
            var referencias = await _dbService.GetTodasLasReferencias();
            Referencias.Clear();
            foreach (var referencia in referencias)
            {
                Referencias.Add(referencia);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
