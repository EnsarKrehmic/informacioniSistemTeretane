using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class ZakazaniGrupni
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

        public DateTime DatumPrijave { get; set; }
    }
}
