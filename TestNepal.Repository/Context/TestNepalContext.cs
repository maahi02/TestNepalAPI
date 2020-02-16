using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using TestNepal.Entities;
using TestNepal.Entities.Common;

namespace TestNepal.Context
{
    public class TestNepalContext : IdentityDbContext<IdentityUser>
    {
        Guid? _currentTenantId = null;
        Guid? _currentUserId = null;
        public TestNepalContext() :
        base("TestNepalDBConnection", throwIfV1Schema: false)
        {
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
        }
        public TestNepalContext(Guid? tenantId, Guid? userId) : base("name=TestNepalDBConnection", throwIfV1Schema: false)
        {
            _currentTenantId = tenantId;
            _currentUserId = userId;
        }
        // basic
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Authentication> Authentications { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Role> TESTRoles { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        // test app table
        public DbSet<Employee> Employee { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Add(new DateTime2Convention());
            modelBuilder.Entity<UserProfile>()
               .HasRequired<ApplicationUser>(profile => profile.User)
               .WithOptional(u => u.UserProfile);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).Property(p => p.Name).IsRequired();
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<IdentityUserLogin>().HasKey(u => new { u.UserId, u.LoginProvider, u.ProviderKey });
            base.OnModelCreating(modelBuilder);

        }
        public static TestNepalContext Create()
        {
            return new TestNepalContext();
        }
        public virtual void Commit()
        {
            base.SaveChanges();
        }
        #region SaveChanges
        public override int SaveChanges()
        {
            //Added Entities with Interface ICreated
            var createdEntities = ChangeTracker.Entries<ICreated>();
            if (createdEntities != null)
            {
                foreach (var item in createdEntities.Where(t => t.State == EntityState.Added))
                {
                    item.Entity.CreatedOn = DateTime.Now;
                    if (_currentUserId.HasValue)
                        item.Entity.CreatedById = _currentUserId.Value;
                }
            }

            //Modified Entities with Interface IUpdated
            var updatedEntities = ChangeTracker.Entries<IUpdated>();
            if (updatedEntities != null)
            {
                foreach (var item in updatedEntities.Where(t => t.State == EntityState.Modified))
                {
                    item.Entity.ModifiedOn = DateTime.Now;
                    if (_currentUserId.HasValue)
                        item.Entity.ModifiedById = _currentUserId;
                }
            }

            //Entities secured by Interface ISecuredByTenant should automatically assign TenantId from context
            var securedByTenantEntities = ChangeTracker.Entries<ISecuredByTenant>();
            foreach (var item in securedByTenantEntities.Where(t => t.State == EntityState.Modified || t.State == EntityState.Added))
            {
                if (_currentTenantId.HasValue)
                    item.Entity.TenantId = _currentTenantId.Value;
            }

            //Entities secured by Interface ISecuredByUser should automatically assign UserId from context
            var securedByUserEntities = ChangeTracker.Entries<ISecuredByUser>();
            foreach (var item in securedByUserEntities.Where(t => t.State == EntityState.Added))
            {
                if (_currentUserId.HasValue)
                    item.Entity.UserId = _currentUserId.Value;
            }

            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        #endregion SaveChanges
    }

    public class DateTime2Convention : Convention
    {
        public DateTime2Convention()
        {
            this.Properties<DateTime>()
                .Configure(c => c.HasColumnType("datetime2"));
        }
    }
}