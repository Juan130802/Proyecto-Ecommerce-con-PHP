using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [SQLite.Table("RegisLaboratorista")]
    public class Laboratorista
    {

        [PrimaryKey]
        [AutoIncrement]
        [SQLite.Column("id")]
        public int Id { get; set; }

        [SQLite.Column("name")]
        public string Name { get; set; }

        [SQLite.Column("cedula")]
        public string Cedula { get; set; }

        [SQLite.Column("email")]
        public string Email { get; set; }

        [SQLite.Column("password")]
        public string Password { get; set; }
    }
}
