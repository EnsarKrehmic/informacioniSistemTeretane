using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class PersonalniTrening : Trening
    {
        [Required]
        public int TrenerId { get; set; }
        [ForeignKey(nameof(TrenerId))]
        public Trener Trener { get; set; }

        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        public Klijent Klijent { get; set; }

        public DateTime Datum { get; set; }
        public TimeSpan Vrijeme { get; set; }

        [MaxLength(500)]
        public string Napredak { get; set; }
    }
}
