using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DLL.Model;
using DLL.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace DLL.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,int,IdentityUserClaim<int>,IdentityUserRole<int>,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions options):base (options)
        {
            
        }

        private const string IsDeletedProperty = "IsDeleted";
        
        private static readonly MethodInfo _propertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(typeof(bool));
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IsoftDelete).IsAssignableFrom(entity.ClrType))
                {
                    entity.AddProperty(IsDeletedProperty, typeof(bool));// must be add deletedAt colam 
                    modelBuilder.Entity(entity.ClrType).HasQueryFilter(GetIsDeletedRestriction(entity.ClrType));
                }
            }
            modelBuilder.Entity<CourseStudent>().HasKey(sc => new { sc.StudentId, sc.CourseId });
            base.OnModelCreating(modelBuilder);
        }
        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var parm = Expression.Parameter(type, "it");
            var prop = Expression.Call(_propertyMethod, parm, Expression.Constant(IsDeletedProperty));
            var condition = Expression.MakeBinary(ExpressionType.Equal, prop, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parm);
            return lambda;
        }

        public override int SaveChanges()
        {
            BeforeSaveChanges();
            return base.SaveChanges();
        }

        private void BeforeSaveChanges()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entity in entries)
            {
                var nowTime = DateTime.Now;
                if (entity.Entity is ITrackable trackable)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            trackable.CreatedAt = nowTime;
                            trackable.UpdatedAt = nowTime;
                            break;
                        case EntityState.Modified:
                            trackable.UpdatedAt = nowTime;
                            break;
                        case EntityState.Deleted:
                            entity.Property(IsDeletedProperty).CurrentValue = true;
                            trackable.UpdatedAt = nowTime;
                            entity.State = EntityState.Modified;
                            break;
                            
                            
                    }
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            BeforeSaveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}