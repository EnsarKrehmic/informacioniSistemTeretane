using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Mapa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv mape je obavezan")]
        [StringLength(100, ErrorMessage = "Naziv ne može biti duži od 100 karaktera")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Adresa je obavezna")]
        [StringLength(200, ErrorMessage = "Adresa ne može biti duža od 200 karaktera")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Geografska širina je obavezna")]
        [Range(-90.0, 90.0, ErrorMessage = "Geografska širina mora biti između -90 i 90")]
        [Display(Name = "Geografska širina")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Geografska dužina je obavezna")]
        [Range(-180.0, 180.0, ErrorMessage = "Geografska dužina mora biti između -180 i 180")]
        [Display(Name = "Geografska dužina")]
        public double Longitude { get; set; }

        [StringLength(500, ErrorMessage = "Opis ne može biti duži od 500 karaktera")]
        public string? Opis { get; set; }

        [Display(Name = "Tip mape")]
        public TipMape Tip { get; set; } = TipMape.Ostalo;
    }

    public enum TipMape
    {
        Teretana,
        [Display(Name = "Bazeni")]
        Bazen,
        [Display(Name = "Sportski tereni")]
        SportskiTereni,
        [Display(Name = "Planinarske staze")]
        PlaninarskeStaze,
        Ostalo
    }
}