using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Disciplina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TakmicenjeId { get; set; }
        [ForeignKey(nameof(TakmicenjeId))]
        public Takmicenje Takmicenje { get; set; }

        [Required, MaxLength(100)]
        public string Naziv { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        public int MaxUcesnika { get; set; }

        public ICollection<Takmicar> Takmicari { get; set; }
    }
}
