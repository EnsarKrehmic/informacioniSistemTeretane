using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace InformacioniSistemTeretane.Models
{
    public class Screenshot
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan")]
        [StringLength(100, ErrorMessage = "Naziv ne može biti duži od 100 karaktera")]
        public string Naziv { get; set; }

        [Display(Name = "Datum kreiranja")]
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;

        // Base64 string za sliku
        public string? Opis { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Molimo odaberite sliku")]
        [Display(Name = "Datoteka slike")]
        [DataType(DataType.Upload)]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Maksimalna veličina slike je 5MB")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" },
            ErrorMessage = "Dozvoljeni formati su: .jpg, .jpeg, .png")]
        public IFormFile DatotekaSlike { get; set; }
    }

    // Customni validacioni atributi
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maksimalna dozvoljena veličina fajla je {_maxFileSize / 1024 / 1024}MB";
        }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Dozvoljene ekstenzije: {string.Join(", ", _extensions)}";
        }
    }
}