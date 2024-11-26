using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [Table("Doctores")]
    public class Doctores
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("DoctorId")]
        public int DoctorId { get; set; }

        [Column("numerodeidentidad")]
        public string NumeroDeIdentidad { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}
