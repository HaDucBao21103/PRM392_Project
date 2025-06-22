using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.CustomerModel;
using Vaccine.API.Models.CustomerModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController:ControllerBase
    {
      
        private readonly UnitOfWork _unitOfWork;
        public CustomerController( UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("get-customer")]
        public IActionResult GetCustomer()
        {
            //var customerList = _unitOfWork.CustomerRepository.Get(includeProperties: "Children");
            //if(customerList == null) 
            //{ 
            //  return NotFound(new { message = "Cannot find customer"});
            //}
            //return Ok(customerList);
            var customers = _unitOfWork.CustomerRepository
        .Get(includeProperties: "Children") //  Load danh sách Children
        .Select(c => new CustomerDTO
        {
            CustomerId = c.CustomerId,
            Name = c.Name,
            Dob = c.Dob,
            Gender = c.Gender,
            Phone = c.Phone,
            Email = c.Email,
            Address = c.Address,
            BloodType = c.BloodType,
            UserName = c.UserName,

            //  Convert Children sang DTO
            Children = c.Children?.Select(child => new ChildDTO
            {
                ChildId = child.ChildId,
                CustomerId = child.CustomerId,
                Name = child.Name,
                Dob = child.Dob,
                Gender = child.Gender,
                BloodType = child.BloodType
            }).ToList()
        }).ToList();

            return Ok(customers);
        }
        //[HttpPost("create-customer")]
        //public IActionResult CreateCustomer(RequestCreateCustomerModel customer)
        //{
        //    if(customer == null)
        //    {
        //        return BadRequest(new { message = "Customer data is required" });
        //    }
        //    if(!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var existingEmail = _unitOfWork.CustomerRepository.Get(filter: x=> x.Email== customer.Email);
        //    if (existingEmail!= null)
        //    {
        //        return BadRequest(new { message = "Email is existed" });
        //    }
        //    var customerEntity = new Customer
        //    {
        //        Name = customer.Name,
        //        Dob = customer.Dob,
        //        Gender = customer.Gender,
        //        Phone = customer.Phone,
        //        Email = customer.Email,
        //        Address = customer.Address,
        //        BloodType = customer.BloodType,
        //        UserName = customer.UserName,
        //        Password = customer.Password
        //    };
        //    _unitOfWork.CustomerRepository.Insert(customerEntity);
        //    _unitOfWork.Save();
        //    return Ok(new { message ="Customer is created successfully"});

        //}
        [HttpPut("update-customer/{id}")]
        public IActionResult UpdateCustomer(int id, RequestUpdateCustomerModel updatedCustomer)
        {
            if (updatedCustomer == null)
            {
                return BadRequest(new { message = "Customer data is required" });
            }

            // Tìm khách hàng theo ID
            var existingCustomer = _unitOfWork.CustomerRepository.GetByID(id);
            if (existingCustomer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            // Cập nhật thông tin khách hàng
            existingCustomer.Name = updatedCustomer.Name;
            //existingCustomer.Dob = updatedCustomer.Dob;
            existingCustomer.Gender = updatedCustomer.Gender; 
            existingCustomer.Phone = updatedCustomer.Phone;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.Address = updatedCustomer.Address;
            existingCustomer.BloodType = updatedCustomer.BloodType;

            // Lưu thay đổi vào database
            _unitOfWork.CustomerRepository.Update(existingCustomer);
            _unitOfWork.Save();

            return Ok(existingCustomer);

        }
        //[HttpDelete("delete-customer/{id}")]
        //public IActionResult DeleteCustomer(int id)
        //{
        //    var cusFound = _unitOfWork.CustomerRepository.GetByID(id);
        //    if(cusFound== null)
        //    {
        //        return BadRequest(new { message = "Customer does not exist" });
        //    }
        //    _unitOfWork.CustomerRepository.Delete(cusFound);
        //    _unitOfWork.Save();
        //    return Ok(new { message = "Delete customer successfully" });
        //}
    }
}
