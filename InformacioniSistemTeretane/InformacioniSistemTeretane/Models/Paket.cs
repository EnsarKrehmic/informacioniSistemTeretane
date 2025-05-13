using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Paket
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Naziv { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        [Required]
        public decimal Cijena { get; set; }

        public int TrajanjeDana { get; set; }

        public ICollection<Uplata> Uplate { get; set; }
    }
}
