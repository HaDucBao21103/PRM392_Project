using Microsoft.EntityFrameworkCore;
using Quartz;
using Vaccine.Repo.Repository;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Jobs
{
    public class DailyJob: IJob
    {
        private readonly UnitOfWork _unitOfWork;
        public DailyJob(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task Execute(IJobExecutionContext context)
        {
            //lấy bệnh nhân sẽ khám vào ngày mai
            var result = _unitOfWork.AppointmentRepository.GetQueryable().Include(x => x.Customer).Where(x => x.AppointmentDate.AddDays(1).Date == DateTime.Now.Date).ToList();
            
            //// gửi mail cho bệnh nhân
            result.ForEach(x => EmailRepository.SendEmail(x?.Customer?.Email, "Thông báo còn một ngày nữa là tới ngày khám bệnh", $"Xin mới ông bà {x.Customer.Name} đến phòng khám lúc {x.AppointmentDate.AddDays(1)}"));

            return Task.CompletedTask;
        }
    }
}
