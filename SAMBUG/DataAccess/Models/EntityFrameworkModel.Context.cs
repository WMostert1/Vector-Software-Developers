﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BugDBEntities : DbContext
    {
        public BugDBEntities()
            : base("name=BugDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<DevicePushNotification> DevicePushNotifications { get; set; }
        public virtual DbSet<Farm> Farms { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ScoutBug> ScoutBugs { get; set; }
        public virtual DbSet<ScoutStop> ScoutStops { get; set; }
        public virtual DbSet<Species> Species { get; set; }
        public virtual DbSet<Treatment> Treatments { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
