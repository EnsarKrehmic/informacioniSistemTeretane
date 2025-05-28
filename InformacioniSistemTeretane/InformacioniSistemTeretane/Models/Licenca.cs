using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Licenca
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Klijent")]
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Required]
        [Display(Name = "Program")]
        public int ProgramId { get; set; }

        [ForeignKey(nameof(ProgramId))]
        [ValidateNever]
        public LicencniProgram Program { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum izdavanja")]
        public DateTime DatumIzdavanja { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Validna do")]
        public DateTime ValidnaDo { get; set; }
    }
}
