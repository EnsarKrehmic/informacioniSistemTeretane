using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class PersonalniTrening : Trening
    {
        [Required]
        [Display(Name = "Trener")]
        public int TrenerId { get; set; }

        [ForeignKey(nameof(TrenerId))]
        [ValidateNever]
        public Trener Trener { get; set; }

        [Required]
        [Display(Name = "Klijent")]
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Display(Name = "Datum treninga")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Display(Name = "Vrijeme treninga")]
        [DataType(DataType.Time)]
        public TimeSpan Vrijeme { get; set; }

        [MaxLength(500)]
        [Display(Name = "Napredak")]
        public string Napredak { get; set; }
    }
}
