﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MagicWarehouse.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MagicEntities : DbContext
    {
        public MagicEntities()
            : base("name=MagicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<A_Device> A_Device { get; set; }
        public virtual DbSet<A_DeviceDocument> A_DeviceDocument { get; set; }
        public virtual DbSet<A_DevicesHistory> A_DevicesHistory { get; set; }
        public virtual DbSet<A_DeviceType> A_DeviceType { get; set; }
        public virtual DbSet<A_Employee> A_Employee { get; set; }
        public virtual DbSet<A_Store> A_Store { get; set; }
    }
}
