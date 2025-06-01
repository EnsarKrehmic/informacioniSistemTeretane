using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class VideoSadrzaj
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv videa je obavezan")]
        [StringLength(100, ErrorMessage = "Naziv ne može biti duži od 100 karaktera")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "YouTube ID je obavezan")]
        [StringLength(11, MinimumLength = 11,
            ErrorMessage = "YouTube ID mora imati tačno 11 karaktera")]
        [RegularExpression(@"^[a-zA-Z0-9_-]{11}$",
            ErrorMessage = "Nevažeći YouTube ID format")]
        [Display(Name = "YouTube Video ID")]
        public string YouTubeVideoId { get; set; }

        [StringLength(500, ErrorMessage = "Opis ne može biti duži od 500 karaktera")]
        public string? Opis { get; set; }

        [Display(Name = "Datum dodavanja")]
        public DateTime DatumDodavanja { get; set; } = DateTime.Now;
    }
}