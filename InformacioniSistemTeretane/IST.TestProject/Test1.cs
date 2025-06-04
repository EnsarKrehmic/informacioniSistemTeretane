using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using InformacioniSistemTeretane.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IST.TestProject
{
    [TestClass]
    public class ModelValidationTests
    {
        // *** 1) Test za Klijent: validan model ***
        [TestMethod]
        public void Klijent_ValidProperties_PassesValidation()
        {
            // Arrange: kreiramo Klijenta sa svim potrebnim poljima
            var klijent = new Klijent
            {
                Id = 1,
                Ime = "Marko",
                Prezime = "Marković",
                DatumRodjenja = new DateTime(1992, 7, 10),
                UserId = "user-abc-123" // obavezno polje
            };

            // Act: izvršavamo validaciju putem DataAnnotations
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(klijent, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(
                klijent,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert: nema vraćenih grešaka
            Assert.IsTrue(isValid, "Očekujemo da je Klijent sa ispravnim svojstvima validan");
            Assert.AreEqual(0, validationResults.Count, "Ne bi smjelo biti validation errora");
        }

        // *** 2) Test za Klijent: nedostaju required polja ***
        [TestMethod]
        public void Klijent_MissingRequiredFields_FailsValidation()
        {
            // Arrange: Kreiramo Klijenta bez Ime, Prezime i UserId
            var klijent = new Klijent
            {
                Id = 2,
                Ime = "",             // Required
                Prezime = null,       // Required
                DatumRodjenja = DateTime.Today,
                UserId = ""           // Required
            };

            // Act: validacija
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(klijent, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(
                klijent,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert: očekujemo barem 3 greške (Ime, Prezime, UserId)
            Assert.IsFalse(isValid, "Model Klijent s praznim obaveznim poljima ne bi trebao biti validan");
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains(nameof(Klijent.Ime))),
                "Treba biti greška za obavezno polje Ime");
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains(nameof(Klijent.Prezime))),
                "Treba biti greška za obavezno polje Prezime");
            Assert.IsTrue(validationResults.Any(vr => vr.MemberNames.Contains(nameof(Klijent.UserId))),
                "Treba biti greška za obavezno polje UserId");
        }

        // *** 3) Test za Takmicar: validan model ***
        [TestMethod]
        public void Takmicar_ValidProperties_PassesValidation()
        {
            // Arrange: Kreiramo Takmicara sa svim required poljima
            var takmicar = new Takmicar
            {
                Id = 1,
                KlijentId = 5,       // Required
                DisciplinaId = 10,   // Required
                Rezultat = 12.5m,
                Pozicija = 1
            };

            // Act: validacija
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(takmicar, serviceProvider: null, items: null);
            bool isValid = Validator.TryValidateObject(
                takmicar,
                context,
                validationResults,
                validateAllProperties: true
            );

            // Assert: nema validation errora
            Assert.IsTrue(isValid, "Takmicar sa ispunjenim required poljima treba biti validan");
            Assert.AreEqual(0, validationResults.Count, "Ne bi smjelo biti validation errora");
        }
    }
}
