using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Uplata
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Klijent")]
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Required]
        [Display(Name = "Paket")]
        public int PaketId { get; set; }

        [ForeignKey(nameof(PaketId))]
        [ValidateNever]
        public Paket Paket { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum uplate")]
        public DateTime DatumUplate { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Način uplate")]
        public string NacinUplate { get; set; }

        [Required]
        [Display(Name = "Iznos")]
        [DataType(DataType.Currency)]
        public decimal Iznos { get; set; }
    }
}
