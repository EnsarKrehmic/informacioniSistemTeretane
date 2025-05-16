using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class IgraonicaPonuda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IgraonicaId { get; set; }

        [ForeignKey(nameof(IgraonicaId))]
        [ValidateNever]
        public Igraonica Igraonica { get; set; }

        [MaxLength(250)]
        public string OpisUsluge { get; set; }

        [Required]
        public decimal Cijena { get; set; }

        public TimeSpan Trajanje { get; set; }
    }
}
