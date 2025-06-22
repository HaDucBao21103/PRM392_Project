using Vaccine.Repo.Repository;
using Vaccine.Repo.Entities;
namespace Vaccine.Repo.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
        Task<int> SaveAsync();


        //public GenericRepository<Admin> AdminRepository { get; }
        //public GenericRepository<Appointment> AppointmentRepository { get; }
        //public GenericRepository<Child> ChildRepository { get; }
        //public GenericRepository<Feedback> FeedbackRepository { get; }
        ////public GenericRepository<Notification> NotificationRepository { get; }
        ////public GenericRepository<PackageDetail> PackageDetailRepository { get; }
        ////public GenericRepository<Payment> PaymentRepository { get; }
        ////public GenericRepository<Role> RoleRepository { get; }
        ////public GenericRepository<ServicePackage> ServicePackageRepository { get; }
        ////public GenericRepository<UserAccount> UserAccountRepository { get; }
        ////public GenericRepository<VaccinationRecord> VaccinationRecordRepository { get; }
        ////public GenericRepository<VaccineReaction> VaccineReactionRepository { get; }

        public GenericRepository<Admin> AdminRepository { get; }
        public GenericRepository<Appointment> AppointmentRepository { get; }
        public GenericRepository<Child> ChildRepository { get; }
        public GenericRepository<Customer> CustomerRepository { get; }
        public GenericRepository<Doctor> DoctorRepository { get; }
        public GenericRepository<Feedback> FeedbackRepository { get; }
        public GenericRepository<HealthRecord> HealthRecordRepository { get; }
        public GenericRepository<Holiday> HolidayRepository { get; }
        public GenericRepository<Invoice> InvoiceRepository { get; }
        public GenericRepository<InvoiceDetail> InvoiceDetailRepository { get; }
        public GenericRepository<Staff> StaffRepository { get; }
        public GenericRepository<Vaccine.Repo.Entities.Vaccine> VaccineRepository { get; }
        public GenericRepository<VaccineBatch> VaccineBatchRepository { get; }
        public GenericRepository<VaccineBatchDetail> VaccineBatchDetailRepository { get; }
        public GenericRepository<VaccineCombo> VaccineComboRepository { get; }
        public GenericRepository<VaccineComboDetail> VaccineComboDetailRepository { get; }


    }
}