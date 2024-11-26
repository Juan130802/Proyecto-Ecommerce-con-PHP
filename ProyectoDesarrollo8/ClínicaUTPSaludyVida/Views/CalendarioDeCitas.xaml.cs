using System.Collections.ObjectModel;

namespace Cl�nica_UTP_Salud_y_Vida.Views;

public partial class CalendarioDeCitas : ContentPage
{
    // Aqu� va el c�digo de tu clase
    public ObservableCollection<DiaModel> DiasDelMes { get; set; }

    public CalendarioDeCitas()
    {
        InitializeComponent();
        DiasDelMes = GenerarDiasDelMes(2024, 9); // Generar d�as para septiembre 2024
        BindingContext = this;
    }

    private ObservableCollection<DiaModel> GenerarDiasDelMes(int year, int month)
    {
        var dias = new ObservableCollection<DiaModel>();
        var primerDia = new DateTime(year, month, 1);
        int diaSemanaInicio = (int)primerDia.DayOfWeek;

        if (diaSemanaInicio == 0)
            diaSemanaInicio = 7; // Si es domingo, lo colocamos como �ltimo d�a de la semana.

        for (int i = 1; i < diaSemanaInicio; i++)
        {
            dias.Add(new DiaModel { Dia = "" }); // D�as vac�os para alinear la cuadr�cula.
        }

        int diasEnElMes = DateTime.DaysInMonth(year, month);
        for (int i = 1; i <= diasEnElMes; i++)
        {
            dias.Add(new DiaModel { Dia = i.ToString() });
        }

        return dias;
    }
}

public class DiaModel
{
    public string Dia { get; set; }
}
