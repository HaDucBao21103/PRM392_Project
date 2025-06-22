using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Vaccine.API.Models.ChildModel;
using Vaccine.API.Models.CustomerModel;
using Vaccine.API.Models.InvoiceModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class ChildController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ChildController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var childs = _unitOfWork.ChildRepository.Get();
            return Ok(childs);
        }

        [HttpGet("get-child/{id}")]
        public IActionResult GetChild(int id)
        {
            var child = _unitOfWork.ChildRepository.Get(d => d.CustomerId == id);
            if (child == null)
            {
                return NotFound(new { message = "Child not found." });
            }
            return Ok(child);
        }

        [HttpPost("create-child")]
        [SwaggerOperation(
        Summary = "Create a child",
        Description = "Creates a new child.")]

        [SwaggerRequestExample(typeof(RequestUpdateChildModel), typeof(ExampleRequestCreateChildModel))]
        public IActionResult CreateChild(RequestUpdateChildModel requestCreateChildModel)
        {
            var childEntity = new Child
            {
                CustomerId =requestCreateChildModel.CustomerId,
                Name = requestCreateChildModel.Name,
                Dob = requestCreateChildModel.Dob,
                Gender = requestCreateChildModel.Gender,
                BloodType = requestCreateChildModel.BloodType
            };
            _unitOfWork.ChildRepository.Insert(childEntity);
            _unitOfWork.Save();
            return Ok(childEntity);
        }

        [HttpPut("update-child/{id}")]
        [SwaggerOperation(
    Summary = "Update a child",
    Description = "Updates an existing child by ID.")]
        public IActionResult UpdateChild(int id, RequestUpdateChildModel request)
        {
            var childEntity = _unitOfWork.ChildRepository.GetByID(id);
            if (childEntity == null)
            {
                return NotFound("Child not found");
            }

            childEntity.CustomerId =  request.CustomerId;
            childEntity.Name = request.Name;
            childEntity.Dob = request.Dob;
            childEntity.Gender = request.Gender;
            childEntity.BloodType = request.BloodType;

            _unitOfWork.ChildRepository.Update(childEntity);
            _unitOfWork.Save();
            return Ok(childEntity);
        }

        [HttpDelete("delete-child/{id}")]
        public IActionResult DeleteChild(int id)
        {
            var childEntity = _unitOfWork.ChildRepository.GetByID(id);
            if (childEntity == null)
            {
                return NotFound("Child not found");
            }
            _unitOfWork.ChildRepository.Delete(childEntity);
            _unitOfWork.Save();
            return Ok(childEntity);
        }
        
    }
}
