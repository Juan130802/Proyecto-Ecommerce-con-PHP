using Clínica_UTP_Salud_y_Vida.Models;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Clínica_UTP_Salud_y_Vida.ViewsModel;
public class ProcedimientoMedicoViewModel : BaseViewModel
{
    private readonly LocalDbService _dbService;
    private PacientesRegister _pacienteSeleccionado;

    public ObservableCollection<PacientesRegister> Pacientes { get; set; }
    public ObservableCollection<string> Procedimientos { get; set; }

    public string CedulaBusqueda { get; set; }
    public string ProcedimientoSeleccionado { get; set; }
    public string Observaciones { get; set; }

    public PacientesRegister PacienteSeleccionado
    {
        get => _pacienteSeleccionado;
        set
        {
            _pacienteSeleccionado = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(NombrePaciente));
            OnPropertyChanged(nameof(CedulaPaciente));
        }
    }

    public string NombrePaciente => PacienteSeleccionado?.Name;
    public string CedulaPaciente => PacienteSeleccionado?.Cedula;

    public ICommand BuscarPacienteCommand { get; }
    public ICommand CrearPacienteCommand { get; }
    public ICommand GuardarProcedimientoCommand { get; }

    public ProcedimientoMedicoViewModel()
    {
        _dbService = new LocalDbService();
        Procedimientos = new ObservableCollection<string>
        {
            "Curaciones", "Corte de puntos", "Control de peso y talla",
            "Control de presión arterial", "Inhaloterapias", "Aplicación de medicamentos inyectables",
            "Toma de glicemia capilar"
        };
        Pacientes = new ObservableCollection<PacientesRegister>();

        BuscarPacienteCommand = new Command(async () => await BuscarPaciente());
        CrearPacienteCommand = new Command(async () => await CrearPaciente());
        GuardarProcedimientoCommand = new Command(async () => await GuardarProcedimiento());
    }

    private async Task BuscarPaciente()
    {
        Pacientes.Clear();
        var pacientes = await _dbService.BuscarPorCedula(CedulaBusqueda);
        foreach (var paciente in pacientes)
        {
            Pacientes.Add(paciente);
        }
    }

    private async Task CrearPaciente()
    {
        var nuevoPaciente = new PacientesRegister { Name = PacienteSeleccionado.Name, Cedula = CedulaBusqueda };
        await _dbService.Create(nuevoPaciente);
    }

    private async Task GuardarProcedimiento()
    {
        if (PacienteSeleccionado != null && !string.IsNullOrEmpty(ProcedimientoSeleccionado))
        {
            try
            {
                var procedimiento = new ProcedimientoMedico
                {
                    PacienteId = PacienteSeleccionado.Id,
                    TipoProcedimiento = ProcedimientoSeleccionado,
                    Fecha = DateTime.Now,
                    Observaciones = Observaciones
                };

                await _dbService.CreateProcedimiento(procedimiento);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Procedimiento guardado correctamente.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error al guardar: {ex.Message}", "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Advertencia", "Por favor, seleccione un paciente y un procedimiento.", "OK");
        }
    }
}
