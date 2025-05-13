using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class IgraonicaPonuda
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IgraonicaId { get; set; }
        [ForeignKey(nameof(IgraonicaId))]
        public Igraonica Igraonica { get; set; }

        [MaxLength(250)]
        public string OpisUsluge { get; set; }

        [Required]
        public decimal Cijena { get; set; }

        public TimeSpan Trajanje { get; set; }
    }
}
