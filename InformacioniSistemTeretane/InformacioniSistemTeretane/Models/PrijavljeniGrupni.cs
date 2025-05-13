using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class PrijavljeniGrupni
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GrupniTreningId { get; set; }
        [ForeignKey(nameof(GrupniTreningId))]
        public GrupniTrening GrupniTrening { get; set; }

        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        public Klijent Klijent { get; set; }

        public bool Prisutan { get; set; }
        public DateTime VrijemeDolaska { get; set; }
    }
}
