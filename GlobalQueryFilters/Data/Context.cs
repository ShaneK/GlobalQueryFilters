using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GlobalQueryFilters.Magic;
using GlobalQueryFilters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GlobalQueryFilters.Data
{
    public class Context : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Topping> Toppings { get; set; }

        // Reflection methods for OnModelCreating
        private static readonly MethodInfo SetGlobalQueryMethod = typeof(Context)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Global query filters
            UseReflectionToAddGlobalFilters(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void UseReflectionToAddGlobalFilters(ModelBuilder modelBuilder)
        {
            // Set rules to all Entities/TenantEntities
            var entities = GetEntityTypes<Entity>();
            foreach (var type in entities)
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { modelBuilder });
            }
        }

        // Find loaded entity types from assemblies that application uses
        private static IList<Type> GetEntityTypes<T>()
        {
            var typeList = new List<Type>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var interfaces = type.GetAllBaseTypes();
                if (interfaces.Contains(typeof(T)))
                {
                    typeList.Add(type);
                }
            }

            return typeList;
        }

        // This method is called for every loaded entity type in OnModelCreating method.
        // Here type is known through generic parameter and we can use EF Core methods.
        public void SetGlobalQuery<T>(ModelBuilder builder) where T : Entity
        {
            builder.Entity<T>().HasQueryFilter(e => e.Active);
        }

        public override int SaveChanges()
        {
            SetEntityModified();
            return base.SaveChanges();
        }

        private void SetEntityModified()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                // Update modified date for all entries
                if (entry.Entity is Entity entity && entry.State == EntityState.Modified)
                {
                    entity.ModifiedDate = DateTime.UtcNow;
                }
            }
        }
    }
}