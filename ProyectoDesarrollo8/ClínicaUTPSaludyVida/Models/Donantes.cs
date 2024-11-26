using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [SQLite.Table("Donantes")]
    public class Donantes
    {
        [PrimaryKey]
        [AutoIncrement]
        [SQLite.Column("Id")]
        public int Id { get; set; }

        [Column("IdDonante")]
        public int IdDonante { get; set; }

        [SQLite.Column("Nombre")]
        public string Nombre { get; set; }

        [SQLite.Column("Cedula")]
        public string Cedula { get; set; }

        [SQLite.Column("Tipo_Sangre")]
        public string TipoSangre { get; set; }

        [SQLite.Column("Fecha1")]
        public DateTime FechaDeExamen { get; set; }

        [SQLite.Column("Status")]
        public string Status { get; set; }

        [SQLite.Column("Fecha")]
        public DateTime Fecha_Donacion { get; set; }

    }
}
