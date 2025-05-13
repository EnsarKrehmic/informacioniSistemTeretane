using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace InformacioniSistemTeretane.Models
{
    // Proširenje za IdentityUser kako bismo mogli dodati navigacione veze
    public class ApplicationUser : IdentityUser
    {
        public Klijent Klijent { get; set; }
        public Zaposlenik Zaposlenik { get; set; }
    }
}
