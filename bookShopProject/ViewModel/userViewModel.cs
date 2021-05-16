using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookShopProject.Models;
namespace bookShopProject.ViewModel
{
    public class userViewModel
    {
    
        public string username { get; set; }
        public string password { get; set; }
        public string Role { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string postCode { get; set; }
        public string street { get; set; }
        public string houseNumber { get; set; }
        public int phoneNumber { get; set; }
    }
}