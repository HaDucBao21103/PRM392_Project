using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    try
        //    {
        //        // Lấy tất cả các role
        //        var roles = _unitOfWork.RoleRepository.Get();

    }
}
