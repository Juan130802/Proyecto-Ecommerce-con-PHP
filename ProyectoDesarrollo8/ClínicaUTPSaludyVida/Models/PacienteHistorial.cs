using SQLite;

namespace Clínica_UTP_Salud_y_Vida.Models
{
    [Table("historial_medico")]
    public class PacienteHistorial
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Column("PacienteId")]
        public int PacienteId { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; }

        [Column("Cedula")]
        public string Cedula { get; set; }

        [Column("Edad")]
        public string Edad { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; }

        [Column("TemperaturaCorporal")]
        public float TemperaturaCorporal { get; set; }

        [Column("FrecuenciaCardiaca")]
        public int FrecuenciaCardiaca { get; set; }

        [Column("FrecuenciaRespiratoria")]
        public int FrecuenciaRespiratoria { get; set; }

        [Column("PresionArterial")]
        public string PresionArterial { get; set; }

        [Column("Oxigeno")]
        public int Oxigeno { get; set; }

        [Column("Peso")]
        public float Peso { get; set; }

        [Column("Altura")]
        public float Altura { get; set; }

        [Column("MotivoConsulta")]
        public string MotivoConsulta { get; set; }

        [Column("EnfermedadesPrevias")]
        public string EnfermedadesPrevias { get; set; }

        [Column("MedicamentosActuales")]
        public string MedicamentosActuales { get; set; }

        [Column("AlergiasConocidas")]
        public string AlergiasConocidas { get; set; }

        [Column("NivelDolor")]
        public int NivelDolor { get; set; }
    }
}
