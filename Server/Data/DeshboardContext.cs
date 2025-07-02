using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EskomAdmin.Server.Models.Deshboard;

namespace EskomAdmin.Server.Data
{
    public partial class DeshboardContext : DbContext
    {
        
        public DeshboardContext()
        {
        }
            
        public DeshboardContext(DbContextOptions<DeshboardContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EskomAdmin.Server.Models.Deshboard.Deshboard>()
              .Property(p => p.id)
              .ValueGeneratedOnAddOrUpdate().UseIdentityColumn()
              .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            this.OnModelBuilding(builder);
        }

        public DbSet<EskomAdmin.Server.Models.Deshboard.Deshboard> Deshboards { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
        




    }
}