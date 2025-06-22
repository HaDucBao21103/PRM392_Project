namespace Vaccine.API.Models.AppointmentModel
{
    public class RequestCreateComboAppointment
    {
        public int CustomerId { get; set; }
        public int ChildId { get; set; }
        public int ComboId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
    }
}
