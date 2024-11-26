using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [Table("usuarios")]
    public class Usuarios
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("Name")]
        public string name { get; set; }

        [Column("IdPaciente")]
        public int IdPaciente { get; set; }

        [Column("Edad")]
        public string edad { get; set; }

        [Column("Telefono")]
        public string telefono { get; set; }

        [Column("Tipo_Consulta")]
        public string tipo_consulta { get; set; }

        [Column("Fecha_Consulta")]
        public DateTime Fecha { get; set; }

        [Column("Hora_Consulta")]
        public TimeSpan Hora { get; set; }

        [Column("Motivo")]
        public string motivo { get; set; }

        [Column("DoctorId")]
        public int DoctorId { get; set; }

        public string FechaHoraFormateada => $"{Fecha:dd/MM/yyyy} {Hora:hh\\:mm}";

        public string HoraFormateada => $"{Hora:hh\\:mm}";

        public string DoctorAsignado { get; internal set; }
    }
}
