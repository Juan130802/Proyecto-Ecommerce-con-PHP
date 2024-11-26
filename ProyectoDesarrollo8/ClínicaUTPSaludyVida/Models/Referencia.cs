using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [Table("referencia")]
    public class Referencia
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int id { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Cedula")]
        public string Cedula { get; set; }

        [Column("Contacto")]
        public string Contacto { get; set; }

        [Column("Motivo")]
        public string Motivo { get; set; }

        [Column("especialidadmedica")]
        public string especialidadmedica { get; set; }
    }
}
