using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [Table("ProcedimientoMedico")]
    public class ProcedimientoMedico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        public string TipoProcedimiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
    }
}