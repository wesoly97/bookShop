using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using bookShopProject.Models;

namespace bookShopProject.ViewModel
{
    public class userViewModel
    {
        [Required(ErrorMessage = "Podaj nazwę użytkownika")]
        public string username { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Podaj Hasło")]
        public string password { get; set; }
        public string Role { get; set; }
        [Required(ErrorMessage = "Podaj Imie!")]
        public string name { get; set; }

        [Required(ErrorMessage = "Podaj Nazwisko!")]
        public string surname { get; set; }

        [Required(ErrorMessage = "Podaj Kod Pocztowy!")]
        [RegularExpression(@"^\d{2}(-\d{3})?$", ErrorMessage = "Nieprawidłowy kod pocztowy!")]
        public string postCode { get; set; }
        [Required(ErrorMessage = "Podaj Adres!")]
        public string street { get; set; }
        [Required(ErrorMessage = "Podaj Numer domu!")]
        public string houseNumber { get; set; }
        [Required(ErrorMessage = "Podaj numer telefonu!")]
        [RegularExpression(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)", ErrorMessage = "Nieprawidłowy Numer telefonu!")]
        public int phoneNumber { get; set; }
    }
}