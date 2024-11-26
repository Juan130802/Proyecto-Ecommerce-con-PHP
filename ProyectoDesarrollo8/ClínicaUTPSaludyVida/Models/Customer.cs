using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [SQLite.Table("customer")]
    public class Customer
    {
        [PrimaryKey]
        [AutoIncrement]
        [SQLite.Column("Id")]
        public int Id { get; set; }

        [Column("PacienteId")]
        public int PacienteId { get; set; }

        [SQLite.Column("Nombre")]
        public string Nombre { get; set; }

        [SQLite.Column("Cedula")]
        public string Cedula { get; set; }

    }
}
