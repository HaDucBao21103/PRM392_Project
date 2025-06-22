using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.HolidayModel
{
    public class RequestCreateHolidayModel
    {
        public int? AdminId { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public string? Reason { get; set; }
    }
}
