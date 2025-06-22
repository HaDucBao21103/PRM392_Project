using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccine.Repo.Entities;

namespace Vaccine.Repo.Repository
{
    public class CustomerRepository
    {
        private readonly VaccineDbContext _context;
        public CustomerRepository(VaccineDbContext context)
        {
            _context = context;
        }
    }
}
