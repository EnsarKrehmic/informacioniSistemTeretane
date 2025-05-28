using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformacioniSistemTeretane.Models
{
    public class ProbniTrening : Trening
    {
        [Required]
        [Display(Name = "Klijent")]
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        public Klijent Klijent { get; set; }

        [Required]
        [Display(Name = "Trener")]
        public int TrenerId { get; set; }

        [ForeignKey(nameof(TrenerId))]
        public Trener Trener { get; set; }

        [Display(Name = "Datum treninga")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Display(Name = "Ocjena")]
        public int Ocjena { get; set; }
    }
}
