using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Licenca
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        public Klijent Klijent { get; set; }

        [Required]
        public int ProgramId { get; set; }
        [ForeignKey(nameof(ProgramId))]
        public LicencniProgram Program { get; set; }

        public DateTime DatumIzdavanja { get; set; }
        public DateTime ValidnaDo { get; set; }
    }
}
