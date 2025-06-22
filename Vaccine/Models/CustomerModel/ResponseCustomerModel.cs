using Vaccine.API.Models.InvoiceModel;

namespace MilkStore.API.Models.CustomerModel
{
    public class ResponseCreateCustomerModel
    {
        public string CustomerName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

    }
    public class ResponseCreateGoogleCustomerModel
    {
        public string CustomerName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

    }
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public DateOnly? Dob { get; set; }
        public string? Gender { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? BloodType { get; set; }
        public string UserName { get; set; }
        public List<ChildDTO>? Children { get; set; }
        //public List<AppointmentDTO>? Appointments { get; set; }
        //public List<FeedbackDTO>? Feedbacks { get; set; }
        //public List<InvoiceDTO>? Invoices { get; set; }
    }


        public class ChildDTO
        {
            public int ChildId { get; set; }
            public int? CustomerId { get; set; } // Để liên kết với Customer
            public string Name { get; set; }
            public DateOnly Dob { get; set; }
            public string? Gender { get; set; }
            public string? BloodType { get; set; }

            // Nếu  muốn lấy danh sách Appointment, có thể thêm dòng này
            //public List<AppointmentDTO>? Appointments { get; set; }
        }

    

}