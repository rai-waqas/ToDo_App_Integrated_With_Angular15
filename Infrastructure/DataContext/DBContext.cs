using Microsoft.EntityFrameworkCore;
using Core.Models;
using Task = Core.Models.Task;

namespace Infrastructure.DataContext
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Tag> Tags { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tags");

                entity.HasIndex(e => e.Name, "tags_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("tasks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Completed).HasColumnName("completed");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.DueDate).HasColumnName("due_date");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Priority)
                    .HasMaxLength(10)
                    .HasColumnName("priority")
                    .HasDefaultValueSql("'low'::character varying");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tasks_user_id_fkey");

                entity.HasMany(d => d.Tags)
                    .WithMany(p => p.Tasks)
                    .UsingEntity<Dictionary<string, object>>(
                        "TaskTags",
                        l => l.HasOne<Tag>().WithMany().HasForeignKey("TagId").HasConstraintName("tasktags_tag_id_fkey"),
                        r => r.HasOne<Task>().WithMany().HasForeignKey("TaskId").HasConstraintName("tasktags_task_id_fkey"),
                        j =>
                        {
                            j.HasKey("TaskId", "TagId").HasName("tasktags_pkey");

                            j.ToTable("tasktags");

                            j.IndexerProperty<int>("TaskId").HasColumnName("task_id");

                            j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
