using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Uplata
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        public Klijent Klijent { get; set; }

        [Required]
        public int PaketId { get; set; }
        [ForeignKey(nameof(PaketId))]
        public Paket Paket { get; set; }

        public DateTime DatumUplate { get; set; }

        [Required, MaxLength(50)]
        public string NacinUplate { get; set; }

        [Required]
        public decimal Iznos { get; set; }
    }
}
