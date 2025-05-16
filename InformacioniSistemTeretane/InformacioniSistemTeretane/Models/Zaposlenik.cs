using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Zaposlenik
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Ime { get; set; }

        [Required, MaxLength(100)]
        public string Prezime { get; set; }

        [Required, MaxLength(50)]
        public string Pozicija { get; set; }

        [MaxLength(20)]
        public string Telefon { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [ValidateNever]
        public ICollection<Trener> TrenerskiPodaci { get; set; } = new List<Trener>();

        [ValidateNever]
        public ICollection<Sudija> Sudije { get; set; } = new List<Sudija>();
    }
}
