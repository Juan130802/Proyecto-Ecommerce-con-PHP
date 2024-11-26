using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    public class Recetas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreDoctor { get; set; }

        public string Especialidad { get; set; }

        public string Medicamento { get; set; }

        public string Dosis { get; set; }

        public int IdPaciente { get; set; }

        public string NombrePaciente { get; set; }
    }
}
