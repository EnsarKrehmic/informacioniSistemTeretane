using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class GrupniTrening : Trening
    {
        [Required]
        public int SalaId { get; set; }
        [ForeignKey(nameof(SalaId))]
        public Sala Sala { get; set; }

        [Required]
        public int TrenerId { get; set; }
        [ForeignKey(nameof(TrenerId))]
        public Trener Trener { get; set; }

        public DateTime Datum { get; set; }
        public TimeSpan Vrijeme { get; set; }
        public int MaxUcesnika { get; set; }

        public ICollection<ZakazaniGrupni> Rezervacije { get; set; }
        public ICollection<PrijavljeniGrupni> Prisustva { get; set; }
    }
}
