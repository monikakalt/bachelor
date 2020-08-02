using Microsoft.EntityFrameworkCore;

namespace museumApi.EF.entities
{
    public partial class MuseumContext : DbContext
    {
        public MuseumContext()
        {
        }

        public MuseumContext(DbContextOptions<MuseumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chronicle> Chronicles { get; set; }
        public virtual DbSet<ClassInfo> Classes { get; set; }
        public virtual DbSet<Graduates> Graduates { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=museum;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Chronicle>(entity =>
            {
                entity.ToTable("chronicles");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.FkUser).HasColumnName("fkUser");

                entity.Property(e => e.FolderUrl)
                    .HasColumnName("folderUrl")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Chronicles)
                    .HasForeignKey(d => d.FkUser)
                    .HasConstraintName("Creates");
            });

            modelBuilder.Entity<ClassInfo>(entity =>
            {
                entity.ToTable("classes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FkTeacher).HasColumnName("fkTeacher");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.HasOne(d => d.FkTeacherNavigation)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.FkTeacher)
                    .HasConstraintName("Leads");
            });

            modelBuilder.Entity<Graduates>(entity =>
            {
                entity.ToTable("graduates");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.FkChronicle });

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FkChronicle).HasColumnName("fkChronicle");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(255);

                entity.HasOne(d => d.FkChronicleNavigation)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.FkChronicle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Consists_of");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("reservations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.End)
                    .HasColumnName("endTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FkUser).HasColumnName("fkUser");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.ReminderSent).HasColumnName("reminderSent");

                entity.Property(e => e.Start)
                    .HasColumnName("startTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Makes");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255);

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.FkClass).HasColumnName("fkClass");

                entity.Property(e => e.FkGraduates).HasColumnName("fkGraduates");

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.Property(e => e.SurnameAfterMarriage)
                    .HasColumnName("surnameAfterMarriage")
                    .HasMaxLength(255);

                entity.Property(e => e.Workplace)
                    .HasColumnName("workplace")
                    .HasMaxLength(255);

                entity.HasOne(d => d.FkClassNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.FkClass)
                    .HasConstraintName("Include");

                entity.HasOne(d => d.FkGraduatesNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.FkGraduates)
                    .HasConstraintName("Has");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("teachers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(255);
                entity.Property(e => e.Subject)
                   .HasColumnName("subject")
                   .HasMaxLength(255);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("passwordHash")
                    .HasMaxLength(1024);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("passwordSalt")
                    .HasMaxLength(1024);
            });
        }
    }
}
