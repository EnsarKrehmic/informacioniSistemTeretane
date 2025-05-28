using System;
using System.Collections.Generic;
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
        [Display(Name = "Igraonica")]
        public int IgraonicaId { get; set; }

        [ForeignKey(nameof(IgraonicaId))]
        [ValidateNever]
        public Igraonica Igraonica { get; set; }

        [MaxLength(250)]
        [Display(Name = "Opis usluge")]
        public string OpisUsluge { get; set; }

        [Required]
        [Display(Name = "Cijena (KM)")]
        [DataType(DataType.Currency)]
        public decimal Cijena { get; set; }

        [Display(Name = "Trajanje")]
        [DataType(DataType.Time)]
        public TimeSpan Trajanje { get; set; }
    }
}
