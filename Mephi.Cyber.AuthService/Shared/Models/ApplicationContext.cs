using Microsoft.EntityFrameworkCore;

namespace Mephi.Cyber.AuthService.Core.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Admin> Admins { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RoleParent> RoleParents { get; set; } = null!;
        public DbSet<RoleBinding> RoleBindings { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<RoleGroup> RoleGroups { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Entity> Entities { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<UserGroup> UserGroups { get; set; } = null!;
         
        public DbSet<Act> Acts { get; set; } = null!;
        public DbSet<Relation> Relations { get; set; } = null!;
        public DbSet<Assignment> Assignments { get; set; }  = null!;
        public DbSet<RelationAct> RelationActs { get; set; }  = null!;
       
            
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();   
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().HasIndex(r =>r.Name).IsUnique();
            modelBuilder.Entity<Group>().HasKey(g => g.Id);
            modelBuilder.Entity<Group>().HasIndex(g =>g.Name).IsUnique();
            modelBuilder.Entity<RoleParent>().HasKey(rp => new { rp.RoleId, rp.ParentId });
            modelBuilder.Entity<Permission>().HasKey(p => p.Id);
            modelBuilder.Entity<Permission>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Project>().HasKey(p => p.Id);
            modelBuilder.Entity<Project>().HasIndex(p => p.Name).IsUnique();
            modelBuilder.Entity<Entity>().HasKey(e => e.Id);
            modelBuilder.Entity<Entity>().HasIndex(e => e.Name).IsUnique();
            modelBuilder.Entity<Act>().HasKey(a => a.Id);
            modelBuilder.Entity<Act>().HasIndex(a => a.Name).IsUnique();
            modelBuilder.Entity<Relation>().HasKey(r => r.Id);
            modelBuilder.Entity<Relation>().HasIndex(e => e.Name).IsUnique();
            modelBuilder.Entity<RoleBinding>().HasKey(rb => new { rb.RoleId, rb.UserId});
            modelBuilder.Entity<RoleGroup>().HasKey(rg => new { rg.RoleId, rg.GroupId});
            modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<RelationAct>().HasKey(ra => new { ra.RelationId, ra.ActId });
            modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });
            modelBuilder.Entity<Assignment>().HasKey(r => r.Id);
        }
    }
}