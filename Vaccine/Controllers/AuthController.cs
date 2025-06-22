using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.CustomerModel;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vaccine.API.Models.CustomerModel;
using Vaccine.API.Models.GoogleModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly UnitOfWork _unitOfWork;
        string accessCode ="https://accounts.google.com/o/oauth2/auth?client_id=1006543489483-mrg7qa1pas18ulb0hvnadiagh8jajghs.apps.googleusercontent.com&response_type=code&approval_prompt=force&access_type=offline&redirect_uri=https://localhost:7090/signin-google&scope=openid email profile https://mail.google.com/ ";
        private object requestCreateCustomerModel;

        public AuthController(IConfiguration config, IHttpClientFactory httpClientFactory, UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("link-google")]
        public IActionResult GetLink()
        {
            return Ok(accessCode);
        }



        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleSignIn([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Authorization code is required.");

            var tokenRequest = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _config["Google:ClientId"] },
                { "client_secret", _config["Google:ClientSecret"] },
                { "redirect_uri", _config["Google:RedirectUri"] },
                { "grant_type", "authorization_code" }
            };

            var response = await _httpClient.PostAsync(
                "https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(tokenRequest));

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Google Response Status Code: {response.StatusCode}");
            Console.WriteLine($"Google Response Headers: {string.Join(", ", response.Headers)}");
            Console.WriteLine($"Google Response: {responseContent}");

            if (!response.IsSuccessStatusCode)
                return BadRequest($"Error getting access token: {responseContent}");

            try
            {
                var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(
                    responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Ok(tokenResponse);
            }
            catch (JsonException)
            {
                return BadRequest("Invalid JSON response from Google.");
            }
        }

        /// <summary>
        /// Create a new customer using Google sign-up.
        /// </summary>
        /// <param name="requestCreateCustomerModel">Customer details</param>
        /// <returns>Created customer information</returns>
        [HttpPost("create-customer-by-google")]
        [SwaggerOperation(
        Summary = "Create a customer",
        Description = "Creates a new customer using Google sign-up."
    )]
        //[SwaggerRequestExample(typeof(RequestCreateCustomerModel), typeof(ExampleCreateCustomerModel))]
        public IActionResult CreateCustomer(RequestCreateGoogleCustomerModel requestCreateCustomerModel)
        {
            // Kiểm tra xem email đã tồn tại chưa
            var existingCustomer = _unitOfWork.CustomerRepository.Get(c => c.Email == requestCreateCustomerModel.Email).FirstOrDefault();
            if (existingCustomer != null)
            {
                return BadRequest(new { message = "Email đã được sử dụng để đăng ký tài khoản khác." });
            }

            // para input to create 
            var customerEntity = new Customer
            {
                Name = requestCreateCustomerModel.Name, // name
                Email = requestCreateCustomerModel.Email, //email
                UserName = requestCreateCustomerModel.Email, // Use email as username
                Password = requestCreateCustomerModel.Password, // payload ( google id)
                Phone = "0000000000" // Use default if not provided
            };

            _unitOfWork.CustomerRepository.Insert(customerEntity);
            _unitOfWork.Save();

            var responseCustomer = new ResponseCreateGoogleCustomerModel
            {
                CustomerName = customerEntity.Name,
                Email = customerEntity.Email,
                UserName = customerEntity.UserName,
                Password = customerEntity.Password,
            };

            return Ok(responseCustomer);
        }


        // -----------------------Login Customer------------------------
        [HttpPost("login")]
        public IActionResult Login(LoginRequest login)
        {

            if (login == null || String.IsNullOrEmpty(login.UserName) || String.IsNullOrEmpty(login.Password))
            {
                return BadRequest(new { message = "Username and password are required" });
            }
            dynamic user = null;
            string userRole;
            string preFix = login.UserName.Substring(0, 3);
            var status = "Active";
            Console.WriteLine(preFix);
            if (preFix == "ST_")
            {
                user = _unitOfWork.StaffRepository.
                    Get(s => s.UserName == login.UserName).
                    FirstOrDefault();
                userRole = user?.Role ?? "Staff";
                status= user.Status;    
                if (user != null)
                {

                    switch (user.Role)
                    {
                        case "Data Entry":
                            userRole = "Data Entry";
                            break;
                        case "Nurse":
                            userRole = "Nurse";
                            break;
                        case "Receptionist":
                            userRole = "Receptionist";
                            break;
                        default:
                            userRole = "Staff"; // Nếu không có role cụ thể, giữ mặc định là "Staff"
                            break;
                    }
                }
            }
            else if (preFix == "AD_")
            {
                user = _unitOfWork.AdminRepository.
                    Get(a => a.UserName == login.UserName).
                    FirstOrDefault();
                userRole = "Admin";
            }
            else
            {
                user = _unitOfWork.CustomerRepository.
                    Get(u => u.UserName == login.UserName).
                    FirstOrDefault();
                userRole = "Customer";
            }


            if (user == null || status =="Inactive")
            {
                return Unauthorized(new { message = "Account does not exist" });
            }
            if (user.Password != login.Password)
            {
                return Unauthorized(new { message = "Password is incorrect" });
            }
            Console.WriteLine(user.Gender);

            var response = new
            {
                Id =(userRole == "Data Entry" || userRole == "Nurse" || userRole == "Receptionist") ? user.StaffId :
                     userRole == "Admin" ? user.AdminId :
                     userRole == "Customer" ? user.CustomerId : (int?)null,
                user.Email,
                user.Name,
                Phone = userRole == "Admin" ? null : user.Phone,
                dob = userRole == "Admin" ? null : user.Dob.ToString("yyyy-MM-dd"),
                user.Gender,
                Role = userRole
            };
            Console.WriteLine(response.ToString());
            if (userRole == "Customer")
            {
                return Ok(new
                {
                    response.Id,
                    response.Email,
                    response.Name,
                    response.Phone,
                    response.Role,
                    response.dob,
                    response.Gender,
                    Address = user.Address,
                    Children = user.Children ?? new List<Child>()
                }); ;
            }

            return Ok(response);

        }


        public class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }



        //=========== Signup Customer =====================
        [HttpPost("signup")]

        public IActionResult Signup(SignupRequest newCustomer)
        {
            if (newCustomer == null || string.IsNullOrEmpty(newCustomer.UserName) ||
                string.IsNullOrEmpty(newCustomer.Password) || string.IsNullOrEmpty(newCustomer.Name) ||
                string.IsNullOrEmpty(newCustomer.Phone))
            {
                return BadRequest(new { message = "Missing required fields." });
            }

            var existingUser = _unitOfWork.CustomerRepository.Get(c => c.UserName == newCustomer.UserName).FirstOrDefault();
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var customer = new Customer
            {
                UserName = newCustomer.UserName,
                Password = newCustomer.Password,
                Name = newCustomer.Name,
                Dob = newCustomer.Dob,
                Gender = newCustomer.Gender,
                Phone = newCustomer.Phone,
                Email = newCustomer.Email,
                Address = newCustomer.Address,
                BloodType = newCustomer.BloodType
            };

            _unitOfWork.CustomerRepository.Insert(customer);
            _unitOfWork.Save();

            return Ok(new { message = "Signup successful", customerId = customer.CustomerId });
        }
        public class SignupRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public DateOnly Dob { get; set; }
            public string? Gender { get; set; }
            public string Phone { get; set; }
            public string? Email { get; set; }
            public string? Address { get; set; }
            public string? BloodType { get; set; }
        }

        //=============================================================================

        [HttpPost("verify-token")]
        public async Task<IActionResult> VerifyGoogleToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.IdToken))
                return BadRequest("ID token is required.");

            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
                return Ok(new {payload.Subject, payload.Email, payload.Name, payload.Picture });
            }
            catch
            {
                return BadRequest("Invalid token.");
            }
        }

        //public class GoogleTokenResponse
        //{
        //    [JsonPropertyName("access_token")]
        //    public string AccessToken { get; set; }

        //    [JsonPropertyName("id_token")]
        //    public string IdToken { get; set; }

        //    [JsonPropertyName("refresh_token")]
        //    public string RefreshToken { get; set; }

        //    [JsonPropertyName("expires_in")]
        //    public int ExpiresIn { get; set; }
        //}

        //public class TokenRequest
        //{
        //    public string IdToken { get; set; }
        //}
    }
}
