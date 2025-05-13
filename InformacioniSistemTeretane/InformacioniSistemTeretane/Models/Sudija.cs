using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Sudija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ZaposlenikId { get; set; }
        [ForeignKey(nameof(ZaposlenikId))]
        public Zaposlenik Zaposlenik { get; set; }

        [Required]
        public int DisciplinaId { get; set; }
        [ForeignKey(nameof(DisciplinaId))]
        public Disciplina Disciplina { get; set; }
    }
}
