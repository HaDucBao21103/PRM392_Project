using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Vaccine.Repo.Entities;

public partial class VaccineDbContext : DbContext
{
    public VaccineDbContext()
    {
    }

    public VaccineDbContext(DbContextOptions<VaccineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Child> Children { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Vaccine> Vaccines { get; set; }

    public virtual DbSet<VaccineBatch> VaccineBatches { get; set; }

    public virtual DbSet<VaccineBatchDetail> VaccineBatchDetails { get; set; }

    public virtual DbSet<VaccineCombo> VaccineCombos { get; set; }

    public virtual DbSet<VaccineComboDetail> VaccineComboDetails { get; set; }

    public virtual DbSet<View1> View1s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__43AA41410762D134");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.UserName, "UQ__Admin__7C9273C4CE7AC103").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admin__AB6E61640DA90AEF").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.BloodType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("blood_type");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__A50828FCD1BE03FF");

            entity.ToTable("Appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("datetime")
                .HasColumnName("appointment_date");
            entity.Property(e => e.ChildId).HasColumnName("child_id");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");
            entity.Property(e => e.VaccineType)
                .HasMaxLength(10)
                .HasColumnName("vaccine_type");

            entity.HasOne(d => d.Child).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ChildId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__child__7A672E12");

            entity.HasOne(d => d.Combo).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__Appointme__combo__7E37BEF6");

            entity.HasOne(d => d.Customer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Appointme__custo__797309D9");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Appointme__docto__7C4F7684");

            entity.HasOne(d => d.Staff).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__Appointme__staff__7B5B524B");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.VaccineId)
                .HasConstraintName("FK__Appointme__vacci__7D439ABD");
        });

        modelBuilder.Entity<Child>(entity =>
        {
            entity.HasKey(e => e.ChildId).HasName("PK__Child__015ADC0554E167CB");

            entity.ToTable("Child");

            entity.Property(e => e.ChildId).HasColumnName("child_id");
            entity.Property(e => e.BloodType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("blood_type");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Customer).WithMany(p => p.Children)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Child__customer___5070F446");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__CD65CB85B3CC7EB1");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserName, "UQ__Customer__7C9273C4FD3B77CE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__AB6E6164E66FD7B3").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Customer__B43B145F12F8E329").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BloodType)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("blood_type");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctor__F3993564172CEFB6");

            entity.ToTable("Doctor");

            entity.HasIndex(e => e.Phone, "UQ__Doctor__B43B145FA4B2ED06").IsUnique();

            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Degree)
                .HasMaxLength(255)
                .HasColumnName("degree");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Feedback__60883D900901AB34");

            entity.ToTable("Feedback");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__appoin__18EBB532");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Feedback__custom__17036CC0");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Feedback__doctor__17F790F9");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__BFCFB4DDFB11FDF3");

            entity.ToTable("HealthRecord");

            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AteBeforeVaccine).HasColumnName("ate_before_vaccine");
            entity.Property(e => e.BloodPressure)
                .HasMaxLength(50)
                .HasColumnName("blood_pressure");
            entity.Property(e => e.ConditionOk).HasColumnName("condition_ok");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.HeartRate)
                .HasMaxLength(50)
                .HasColumnName("heart_rate");
            entity.Property(e => e.Height)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("height");
            entity.Property(e => e.ReactionNotes).HasColumnName("reaction_notes");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Temperature)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("temperature");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");

            entity.HasOne(d => d.Appointment).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HealthRec__appoi__10566F31");

            entity.HasOne(d => d.Doctor).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__HealthRec__docto__114A936A");

            entity.HasOne(d => d.Staff).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.StaffId)
                .HasConstraintName("FK__HealthRec__staff__0F624AF8");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.HolidayId).HasName("PK__Holiday__253884EA554EBFD8");

            entity.ToTable("Holiday");

            entity.Property(e => e.HolidayId).HasColumnName("holiday_id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.DateFrom).HasColumnName("date_from");
            entity.Property(e => e.DateTo).HasColumnName("date_to");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");

            entity.HasOne(d => d.Admin).WithMany(p => p.Holidays)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK__Holiday__admin_i__1BC821DD");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__Invoice__F58DFD49D7A5F49A");

            entity.ToTable("Invoice");

            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoice__custome__04E4BC85");
        });

        modelBuilder.Entity<InvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__InvoiceD__38E9A2249E45BF8D");

            entity.ToTable("InvoiceDetail");

            entity.Property(e => e.DetailId).HasColumnName("detail_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");

            entity.HasOne(d => d.Appointment).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__InvoiceDe__appoi__0A9D95DB");

            entity.HasOne(d => d.Combo).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__InvoiceDe__combo__0B91BA14");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceDe__invoi__08B54D69");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.InvoiceDetails)
                .HasForeignKey(d => d.VaccineId)
                .HasConstraintName("FK__InvoiceDe__vacci__09A971A2");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__1963DD9C0AA7A970");

            entity.HasIndex(e => e.UserName, "UQ__Staff__7C9273C46FC1EF5D").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Staff__AB6E6164FA676847").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Staff__B43B145FD1FB6E4A").IsUnique();

            entity.Property(e => e.StaffId).HasColumnName("staff_id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(e => e.VaccineId).HasName("PK__Vaccine__B593EFB3BA6C7231");

            entity.ToTable("Vaccine");

            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.InternalDurationDoses).HasColumnName("internal_duration_doses");
            entity.Property(e => e.MaxLateDate).HasColumnName("max_late_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<VaccineBatch>(entity =>
        {
            entity.HasKey(e => e.BatchNumber).HasName("PK__VaccineB__56E37836E0198931");

            entity.ToTable("VaccineBatch");

            entity.Property(e => e.BatchNumber)
                .HasMaxLength(50)
                .HasColumnName("batch_number");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExpiryDate).HasColumnName("expiry_date");
            entity.Property(e => e.ManufactureDate).HasColumnName("manufacture_date");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(255)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
        });

        modelBuilder.Entity<VaccineBatchDetail>(entity =>
        {
            entity.HasKey(e => new { e.BatchNumber, e.VaccineId }).HasName("PK__VaccineB__7DBA46CDCF71F282");

            entity.ToTable("VaccineBatchDetail");

            entity.Property(e => e.BatchNumber)
                .HasMaxLength(50)
                .HasColumnName("batch_number");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");
            entity.Property(e => e.PreOrderQuantity).HasDefaultValue(0);
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.BatchNumberNavigation).WithMany(p => p.VaccineBatchDetails)
                .HasForeignKey(d => d.BatchNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VaccineBa__batch__6D0D32F4");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccineBatchDetails)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VaccineBa__vacci__6E01572D");
        });

        modelBuilder.Entity<VaccineCombo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("PK__VaccineC__18F74AA38174A0F0");

            entity.ToTable("VaccineCombo");

            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<VaccineComboDetail>(entity =>
        {
            entity.HasKey(e => new { e.ComboId, e.VaccineId }).HasName("PK__VaccineC__33AE7458BEE943E9");

            entity.ToTable("VaccineComboDetail");

            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Combo).WithMany(p => p.VaccineComboDetails)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VaccineCo__combo__72C60C4A");

            entity.HasOne(d => d.Vaccine).WithMany(p => p.VaccineComboDetails)
                .HasForeignKey(d => d.VaccineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VaccineCo__vacci__73BA3083");
        });

        modelBuilder.Entity<View1>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_1");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ComboId).HasColumnName("combo_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DetailId).HasColumnName("detail_id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VaccineId).HasColumnName("vaccine_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
