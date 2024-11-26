namespace Clínica_UTP_Salud_y_Vida.Models
{
    public class ProcedimientoMedicoExtendido
    {
        public int Id { get; set; }
        public string TipoProcedimiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public string NombrePaciente { get; set; }
        public string CedulaPaciente { get; set; }
    }

}
