using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UMS.Core.Models;

namespace UMS.Infrastructure;

public partial class UmsdbContext : IdentityDbContext<AuthenticationUser>
{
    public UmsdbContext()
    {
    }

    public UmsdbContext(DbContextOptions<UmsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Httplog> Httplogs { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentSubject> StudentSubjects { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;User Id=sa;Password=1234AAAAAAAAaaaaaaaa@;Database=UMSDB;TrustServerCertificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Httplog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HTTPLog__3214EC070F8369E2");

            entity.ToTable("HTTPLog");

            entity.Property(e => e.Body).IsUnicode(false);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Request)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC27C7DD198E");

            entity.ToTable("Student");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorUserId)
                .HasMaxLength(450)
                .HasColumnName("CreatorUserID");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionUserId)
                .HasMaxLength(450)
                .HasColumnName("DeletionUserID");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationUserId)
                .HasMaxLength(450)
                .HasColumnName("ModificationUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<StudentSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StudentS__3214EC278511DDD5");

            entity.ToTable("StudentSubject");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorUserId)
                .HasMaxLength(450)
                .HasColumnName("CreatorUserID");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionUserId)
                .HasMaxLength(450)
                .HasColumnName("DeletionUserID");
            entity.Property(e => e.Grade).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationUserId)
                .HasMaxLength(450)
                .HasColumnName("ModificationUserID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentSubjects)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSubject_Student");

            entity.HasOne(d => d.Subject).WithMany(p => p.StudentSubjects)
                .HasForeignKey(d => d.SubjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSubject_Subject");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subject__3214EC273B8ED1EA");

            entity.ToTable("Subject");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorUserId)
                .HasMaxLength(450)
                .HasColumnName("CreatorUserID");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionUserId)
                .HasMaxLength(450)
                .HasColumnName("DeletionUserID");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationUserId)
                .HasMaxLength(450)
                .HasColumnName("ModificationUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Subject_Teacher");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teacher__3214EC27913AC36C");

            entity.ToTable("Teacher");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.CreatorUserId)
                .HasMaxLength(450)
                .HasColumnName("CreatorUserID");
            entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            entity.Property(e => e.DeletionUserId)
                .HasMaxLength(450)
                .HasColumnName("DeletionUserID");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationUserId)
                .HasMaxLength(450)
                .HasColumnName("ModificationUserID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC2739D3F214");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "IX_User").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
