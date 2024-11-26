using Clínica_UTP_Salud_y_Vida.Models;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel;
public class AdminProcedimientosViewModel : BaseViewModel
{
    private readonly LocalDbService _dbService;

    public ObservableCollection<ProcedimientoMedicoExtendido> Procedimientos { get; set; }

    public ICommand CargarProcedimientosCommand { get; }

    public AdminProcedimientosViewModel()
    {
        _dbService = new LocalDbService();
        Procedimientos = new ObservableCollection<ProcedimientoMedicoExtendido>();
        CargarProcedimientosCommand = new Command(async () => await CargarProcedimientos());
    }

    private async Task CargarProcedimientos()
    {
        try
        {
            Console.WriteLine("Cargando procedimientos...");
            Procedimientos.Clear();
            var procedimientos = await _dbService.ObtenerTodosLosProcedimientos();
            Console.WriteLine($"Procedimientos cargados: {procedimientos.Count}");

            foreach (var procedimiento in procedimientos)
            {
                Procedimientos.Add(procedimiento);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            Console.WriteLine($"Error al cargar procedimientos: {ex.Message}");
        }
    }

}