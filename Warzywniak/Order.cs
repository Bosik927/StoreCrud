//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warzywniak
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
        }
    
        public int OrderId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> OrdareDate { get; set; }
        public Nullable<bool> Realized { get; set; }
        public byte[] RowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual User User { get; set; }
    }

    public partial class OrderEntity
    {
        public int OrderId { get; set; }
        public List<ProductEntity> products { get; set; }
        public DateTime OrderDate { get; set; }

        public OrderEntity(int OrderId, List<ProductEntity> products, DateTime OrderDate)
        {
            this.OrderId = OrderId;
            this.products = products;
            this.OrderDate = OrderDate;
        }
    }
}
