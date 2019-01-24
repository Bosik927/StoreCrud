//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
namespace Warzywniak
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
            this.Warehouses = new HashSet<Warehouse>();
        }
    
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
		[Required]
        public string ProductUnit { get; set; }
		[RegularExpression("5|8|23",ErrorMessage = "Vat rate must be 5, 8 or 23")]
        public int Vat { get; set; }
        public Nullable<bool> ForDelete { get; set; }
        public byte[] RowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }

    public partial class ProductEntity
    {
        public string ProductName { get; set; }
        public Decimal Quantity { get; set; }
        public string ProductUnit { get; set; }
        public int Vat { get; set; }
        public Decimal ProductPrice { get; set; }
    }
	public enum ProductUnit
	{
		kilogram,
		litr,
		sztuka
    }
}