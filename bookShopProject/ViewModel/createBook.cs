using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bookShopProject.Models;

namespace bookShopProject.ViewModel
{
    public class createBook
    {
        public int id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string publisher { get; set; }
        public int quantity { get; set; }
        public decimal Price { get; set; }
        public int Category_id { get; set; }
        public string url { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cart> cart { get; set; }
        public virtual Category Category { get; set; }
        public List<Category> Categories { get; set; }
    }
}