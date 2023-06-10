using Microsoft.EntityFrameworkCore;

namespace API.DAL;

public partial class RestApiDormitoryContext : DbContext
{
    public RestApiDormitoryContext ()
    {
    }

    public RestApiDormitoryContext (DbContextOptions<RestApiDormitoryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Announcement> Announcements { get; set; } = null!;
    public virtual DbSet<Application> Applications { get; set; } = null!;
    public virtual DbSet<Dormitory> Dormitories { get; set; } = null!;
    public virtual DbSet<Room> Rooms { get; set; } = null!;
    public virtual DbSet<RoomStudent> RoomStudents { get; set; } = null!;
    public virtual DbSet<Student> Students { get; set; } = null!;

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(50);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.Property(e => e.PublishedDate).HasColumnType("datetime");

            entity.Property(e => e.Title).HasMaxLength(20);

            entity.HasOne(d => d.Room)
                .WithMany(p => p.Announcements)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Announcement");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasIndex(e => e.AnnouncementId, "IX_Applications_AnnouncementId");

            entity.HasIndex(e => e.StudentId, "IX_Applications_StudentId");

            entity.Property(e => e.ApplicationDate).HasColumnType("datetime");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Announcement)
                .WithMany(p => p.Applications)
                .HasForeignKey(d => d.AnnouncementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Announcements");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.Applications)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Students");
        });

        modelBuilder.Entity<Dormitory>(entity =>
        {
            entity.Property(e => e.Code).HasMaxLength(20);

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.DormitoryId, "IX_Rooms_DormitoryId");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Dormitory)
                .WithMany(p => p.Rooms)
                .HasForeignKey(d => d.DormitoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rooms_Dormitories");
        });

        modelBuilder.Entity<RoomStudent>(entity =>
        {
            entity.ToTable("RoomStudent");

            entity.HasIndex(e => e.RoomId, "IX_RoomStudent_RoomId");

            entity.HasIndex(e => e.StudentId, "IX_RoomStudent_StudentId");

            entity.Property(e => e.EndDate).HasColumnType("date");

            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.Room)
                .WithMany(p => p.RoomStudents)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomStudent_Rooms");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.RoomStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomStudent_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial (ModelBuilder modelBuilder);
}
