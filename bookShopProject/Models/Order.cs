//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bookShopProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int id { get; set; }
        public string status { get; set; }
        public Nullable<decimal> totalAmount { get; set; }
        public Nullable<int> books_id { get; set; }
        public string books_quantity { get; set; }
        public int User_id { get; set; }
        public string order_number { get; set; }
    
        public virtual books books { get; set; }
    }
}
