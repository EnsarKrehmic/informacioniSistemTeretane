using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Trener
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ZaposlenikId { get; set; }
        [ForeignKey(nameof(ZaposlenikId))]
        public Zaposlenik Zaposlenik { get; set; }

        public ICollection<GrupniTrening> GrupniTreninzi { get; set; }
        public ICollection<PersonalniTrening> PersonalniTreninzi { get; set; }
        public ICollection<ProbniTrening> ProbniTreninzi { get; set; }
    }
}
