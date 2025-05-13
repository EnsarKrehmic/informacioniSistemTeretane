using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class ProbniTrening : Trening
    {
        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        public Klijent Klijent { get; set; }

        [Required]
        public int TrenerId { get; set; }
        [ForeignKey(nameof(TrenerId))]
        public Trener Trener { get; set; }

        public DateTime Datum { get; set; }
        public int Ocjena { get; set; }
    }
}
