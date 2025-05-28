// Models/GrupniTrening.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class GrupniTrening : Trening
    {
        [Required]
        [Display(Name = "Sala")]
        public int SalaId { get; set; }

        [ForeignKey(nameof(SalaId))]
        [ValidateNever]
        public Sala Sala { get; set; }

        [Required]
        [Display(Name = "Trener")]
        public int TrenerId { get; set; }

        [ForeignKey(nameof(TrenerId))]
        [ValidateNever]
        public Trener Trener { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum treninga")]
        public DateTime Datum { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Vrijeme treninga")]
        public TimeSpan Vrijeme { get; set; }

        [Display(Name = "Maksimalni broj učesnika")]
        public int MaxUcesnika { get; set; }

        [ValidateNever]
        public ICollection<ZakazaniGrupni> Rezervacije { get; set; } = new List<ZakazaniGrupni>();

        [ValidateNever]
        public ICollection<PrijavljeniGrupni> Prisustva { get; set; } = new List<PrijavljeniGrupni>();
    }
}
