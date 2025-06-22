using Vaccine.Repo.Repository;
using Vaccine.Repo.Entities;

namespace Vaccine.Repo.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private VaccineDbContext _context;
        private GenericRepository<Admin> _admin;
        private GenericRepository<Appointment> _appointment;
        private GenericRepository<Child> _child;
        private GenericRepository<Customer> _customer;
        private GenericRepository<Doctor> _doctor;
        private GenericRepository<Feedback> _feedback;
        private GenericRepository<HealthRecord> _healthRecord;
        private GenericRepository<Holiday> _holiday;
        private GenericRepository<Invoice> _invoice;
        private GenericRepository<InvoiceDetail> _invoiceDetail;
        private GenericRepository<Staff> _staff;
        private GenericRepository<Vaccine.Repo.Entities.Vaccine> _vaccine;
        private GenericRepository<VaccineBatch> _vaccineBatch;
        private GenericRepository<VaccineBatchDetail> _vaccineBatchDetail;
        private GenericRepository<VaccineCombo> _vaccineCombo;
        private GenericRepository<VaccineComboDetail> _vaccineComboDetail;

        public UnitOfWork(VaccineDbContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // property IUnitOfWork ket noi voi repo thong qua UnitOfWork

        public GenericRepository<Admin> AdminRepository
        {
            get
            {
                if (_admin == null)
                {
                    _admin = new GenericRepository<Admin>(_context);
                }
                return _admin;
            }
        }


        public GenericRepository<Appointment> AppointmentRepository
        {
            get
            {
                if (_appointment == null)
                {
                    _appointment = new GenericRepository<Appointment>(_context);
                }
                return _appointment;
            }
        }

        public GenericRepository<Child> ChildRepository
        {
            get
            {
                if (_child == null)
                {
                    _child = new GenericRepository<Child>(_context);
                }
                return _child;
            }
        }

        public GenericRepository<Feedback> FeedbackRepository
        {
            get
            {
                if (_feedback == null)
                {
                    _feedback = new GenericRepository<Feedback>(_context);
                }
                return _feedback;
            }
        }

       
        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new GenericRepository<Customer>(_context);
                }
                return _customer;
            }
        }

        public GenericRepository<Doctor> DoctorRepository
        {
            get
            {
                if (_doctor == null)
                {
                    _doctor = new GenericRepository<Doctor>(_context);
                }
                return _doctor;
            }
        }

        public GenericRepository<HealthRecord> HealthRecordRepository
        {
            get
            {
                if (_healthRecord == null)
                {
                    _healthRecord = new GenericRepository<HealthRecord>(_context);
                }
                return _healthRecord;
            }
        }
        public GenericRepository<Holiday> HolidayRepository
        {
            get
            {
                if (_holiday == null)
                {
                    _holiday = new GenericRepository<Holiday>(_context);
                }
                return _holiday;
            }
        }
        public GenericRepository<Invoice> InvoiceRepository
        {
            get
            {
                if (_invoice == null)
                {
                    _invoice = new GenericRepository<Invoice>(_context);
                }
                return _invoice;
            }
        }

        public GenericRepository<InvoiceDetail> InvoiceDetailRepository
        {
            get
            {
                if (_invoiceDetail == null)
                {
                    _invoiceDetail = new GenericRepository<InvoiceDetail>(_context);
                }
                return _invoiceDetail;
            }
        }


        public GenericRepository<Staff> StaffRepository
        {
            get
            {
                if (_staff == null)
                {
                    _staff = new GenericRepository<Staff>(_context);
                }
                return _staff;
            }
        }

        public GenericRepository<VaccineBatch> VaccineBatchRepository
        {
            get
            {
                if (_vaccineBatch == null)
                {
                    _vaccineBatch = new GenericRepository<VaccineBatch>(_context);
                }
                return _vaccineBatch;
            }
        }


        public GenericRepository<VaccineBatchDetail> VaccineBatchDetailRepository
        {
            get
            {
                if (_vaccineBatchDetail == null)
                {
                    _vaccineBatchDetail = new GenericRepository<VaccineBatchDetail>(_context);
                }
                return _vaccineBatchDetail;
            }
        }

        public GenericRepository<VaccineCombo> VaccineComboRepository
        {
            get
            {
                if (_vaccineCombo == null)
                {
                    _vaccineCombo = new GenericRepository<VaccineCombo>(_context);
                }
                return _vaccineCombo;
            }
        }

        public GenericRepository<VaccineComboDetail> VaccineComboDetailRepository
        {
            get
            {
                if (_vaccineComboDetail == null)
                {
                    _vaccineComboDetail = new GenericRepository<VaccineComboDetail>(_context);
                }
                return _vaccineComboDetail;
            }
        }

        public GenericRepository<Vaccine.Repo.Entities.Vaccine> VaccineRepository
        {
            get
            {
                if (_vaccine == null)
                {
                    _vaccine = new GenericRepository<Vaccine.Repo.Entities.Vaccine>(_context);
                }
                return _vaccine;
            }
        }
    }
}
