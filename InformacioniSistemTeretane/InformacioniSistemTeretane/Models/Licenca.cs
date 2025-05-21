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
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [ForeignKey(nameof(ProgramId))]
        [ValidateNever]
        public LicencniProgram Program { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumIzdavanja { get; set; }

        [DataType(DataType.Date)]
        public DateTime ValidnaDo { get; set; }
    }
}
