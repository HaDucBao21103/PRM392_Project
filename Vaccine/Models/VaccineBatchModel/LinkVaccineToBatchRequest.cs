using Swashbuckle.AspNetCore.Filters;

namespace Vaccine.API.Models.VaccineBatchModel
{
    public class LinkVaccineToBatchRequest
    {
        public int VaccineId { get; set; }
        public int Quantity { get; set; }
        public string BatchId { get; set; }
    }
    public class LinkVaccineToBatchExample : IExamplesProvider<LinkVaccineToBatchRequest>
    {
        public LinkVaccineToBatchRequest GetExamples()
        {
            return new LinkVaccineToBatchRequest
            {
                VaccineId = 2,
                Quantity = 100,
                BatchId = "BATCH2024001"
            };
        }
    }


}
