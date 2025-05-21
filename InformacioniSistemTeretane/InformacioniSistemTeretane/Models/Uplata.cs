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
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Required]
        public int PaketId { get; set; }

        [ForeignKey(nameof(PaketId))]
        [ValidateNever]
        public Paket Paket { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumUplate { get; set; }

        [Required, MaxLength(50)]
        public string NacinUplate { get; set; }

        [Required]
        public decimal Iznos { get; set; }
    }
}
