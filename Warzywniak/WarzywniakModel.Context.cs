﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class WarzywniakEntities : DbContext
    {
        public WarzywniakEntities()
            : base("name=WarzywniakEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Adress> Adresses { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
    
        public virtual int DailyExceededExpiryDate()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DailyExceededExpiryDate");
        }
    
        public virtual ObjectResult<Nullable<decimal>> GetAllOrdersValueForUserId(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("GetAllOrdersValueForUserId", userIdParameter);
        }
    
        public virtual ObjectResult<GetAllProductsForUserId_Result> GetAllProductsForUserId(Nullable<int> userId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllProductsForUserId_Result>("GetAllProductsForUserId", userIdParameter);
        }
    
        //public virtual ObjectResult<SaleSummary_Result> SaleSummary(Nullable<System.DateTime> beginDate, Nullable<System.DateTime> endDate)
        //{
        //    var beginDateParameter = beginDate.HasValue ?
        //        new ObjectParameter("BeginDate", beginDate) :
        //        new ObjectParameter("BeginDate", typeof(System.DateTime));
    
        //    var endDateParameter = endDate.HasValue ?
        //        new ObjectParameter("EndDate", endDate) :
        //        new ObjectParameter("EndDate", typeof(System.DateTime));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SaleSummary_Result>("SaleSummary", beginDateParameter, endDateParameter);
        //}
    
        //public virtual ObjectResult<SaleSummaryRealized_Result> SaleSummaryRealized(Nullable<System.DateTime> beginDate, Nullable<System.DateTime> endDate)
        //{
        //    var beginDateParameter = beginDate.HasValue ?
        //        new ObjectParameter("BeginDate", beginDate) :
        //        new ObjectParameter("BeginDate", typeof(System.DateTime));
    
        //    var endDateParameter = endDate.HasValue ?
        //        new ObjectParameter("EndDate", endDate) :
        //        new ObjectParameter("EndDate", typeof(System.DateTime));
    
        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SaleSummaryRealized_Result>("SaleSummaryRealized", beginDateParameter, endDateParameter);
        //}
    
        public virtual int DeltingUsersProductsForDelete()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeltingUsersProductsForDelete");
        }
    
        public virtual ObjectResult<SaleSummaryN_Result> SaleSummaryN(Nullable<System.DateTime> beginDate, Nullable<System.DateTime> endDate)
        {
            var beginDateParameter = beginDate.HasValue ?
                new ObjectParameter("BeginDate", beginDate) :
                new ObjectParameter("BeginDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SaleSummaryN_Result>("SaleSummaryN", beginDateParameter, endDateParameter);
        }
    
        public virtual ObjectResult<SaleSummaryRealizedN_Result> SaleSummaryRealizedN(Nullable<System.DateTime> beginDate, Nullable<System.DateTime> endDate)
        {
            var beginDateParameter = beginDate.HasValue ?
                new ObjectParameter("BeginDate", beginDate) :
                new ObjectParameter("BeginDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("EndDate", endDate) :
                new ObjectParameter("EndDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SaleSummaryRealizedN_Result>("SaleSummaryRealizedN", beginDateParameter, endDateParameter);
        }
    
        public virtual ObjectResult<BestClient_Result> BestClient(Nullable<int> amount)
        {
            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BestClient_Result>("BestClient", amountParameter);
        }
    
        public virtual ObjectResult<BestSellingProduct_Result> BestSellingProduct(Nullable<int> amount, Nullable<bool> sold)
        {
            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(int));
    
            var soldParameter = sold.HasValue ?
                new ObjectParameter("Sold", sold) :
                new ObjectParameter("Sold", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BestSellingProduct_Result>("BestSellingProduct", amountParameter, soldParameter);
        }
    }
}
