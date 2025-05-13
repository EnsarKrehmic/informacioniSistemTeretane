using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        public ApplicationUser User { get; set; }

        public ICollection<Trener> TrenerskiPodaci { get; set; }
        public ICollection<Sudija> Sudije { get; set; }
    }
}
