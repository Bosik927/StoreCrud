//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Warzywniak
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Adresses = new HashSet<Adress>();
            this.Orders = new HashSet<Order>();
        }
    
        public int UserId { get; set; }
        [Required(ErrorMessage = "Nick cannot be empty!")]
        public string Nick { get; set; }
        [Required(ErrorMessage = "Password cannot be empty!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone number cannot be empty!")]
        public int PhoneNumber { get; set; }
        [RegularExpression(".+[@].+\\.(com|pl)", ErrorMessage = "Wrong email format! Needed format like: something@something.com! Avaliable extension: pl, com")]
        public string EmailAddress { get; set; }
        public Nullable<bool> ForDelete { get; set; }
        public byte[] RowVersion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adress> Adresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class UserAdress
    {
        public User user { get; set; }
        public Adress adress { get; set; }

        public UserAdress(User user, Adress adress)
        {
            this.user = user;
            this.adress = adress;
        }

    }
}