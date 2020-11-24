using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PetClinicWebApi.Model.Entity;
using PetClinicWebApi.Model.Identity;
using PetClinicWebApi.Helper;
using PetClinicWebApi.Services;
using PetClinicWebApi.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace PetClinicWebApi.Controllers
{
   
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly PetService _petService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, PetService petService, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _petService = petService;
            _roleManager = roleManager;
        }
        //[HttpGet]
        //public ActionResult<List<Pet>> Get() =>
        //   _petService.Get();
        // GET api/user/userdata
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult> UserData()
        {
            var user = await _userManager.GetUserAsync(User);
            var userData = new UserDataResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,             
                Email = user.Email
            };
            return Ok(userData);
        }
        [HttpGet]
        public async Task<ActionResult> FindByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userData = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PetName = user.PetName,
                Address = user.Address,
                Age = user.Age,
                Gender = user.Gender

            };
            return Ok(userData);
        }
        [HttpGet]
        public async Task<ActionResult> AfterUserLogin()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userData = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PetName = user.PetName,
                Address = user.Address,
                Age = user.Age,
                Gender = user.Gender

            };
            return Ok(userData);
        }

        // POST api/user/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterEntity model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, Email = model.Email };
                user.Roles.Add("5fb57ed576596a7d5118b946");
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    
                    //var test = await _userManager.IsInRoleAsync(user, "NormalUser");
                    await _signInManager.SignInAsync(user, false);
                    var token = AuthenticationHelper.GenerateJwtToken(model.Email, user, _configuration);

                    var rootData = new SignUpResponse(token, user.UserName, user.Email);
                    return Created("api/v1/authentication/register", rootData);
                }
                return Ok(string.Join(",", result.Errors?.Select(error => error.Description)));
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }


        // POST api/user/login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginEntity model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    var token = AuthenticationHelper.GenerateJwtToken(model.Email, appUser, _configuration);

                    var rootData = new LoginResponse(token, appUser.UserName, appUser.Email);
                    return Ok(rootData);
                }
                return StatusCode((int)HttpStatusCode.Unauthorized, "Bad Credentials");
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }

        [HttpGet]
        public ActionResult GetUserList()
        {
            var user =  _userManager.Users.ToList();
            return Ok(user);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int index)
        {
                try
                {
                    var userList =  _userManager.Users.ToList();
                    var user = userList[index];
                if(user != null)
                {
                    await _userManager.DeleteAsync(user);
                    return Ok();
                }
                    

                }
                catch(Exception ex)
                {

                }
            
            return NotFound();
            
        }

        [HttpGet]
        public async Task<ActionResult> UpdateUserInfo(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var model = new EditUserModel()
                {
                    UserId = user.Id.ToString(),
                    Address = user.Address,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    PetName = user.PetName,
                    Gender = user.Gender,
                    ContactPhone = user.ContactPhone
                    

                };
                return Ok(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> UpdateUserInfo(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                try
                {
                    user.Address = model.Address;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PetName = model.PetName;
                    user.Age = model.Age;
                    user.ContactPhone = model.ContactPhone;
                    await _userManager.UpdateAsync(user);
                    return Ok();
                }
                catch
                {

                }
            }
            return Ok();
        }
        public async Task<ActionResult> ManageUserRole(int index)
        {
            var userList = _userManager.Users.ToList();
            
            var roles = _roleManager.Roles.ToList();
            var users = _userManager.Users.ToList();
            if(roles == null)
            {
                
                
                    return NotFound();
                
            }

            var role = roles[index];
            var model = new List<UserRoleModel>();
            foreach (var user in users)
            {
                var userRole = new UserRoleModel
                {
                    UserId = user.Id.ToString(),
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }
                model.Add(userRole);
            }
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRole(int index, List<UserRoleModel> model)
        {
           // var userList = _userManager.Users.ToList();
            var roles =  _roleManager.Roles.ToList();
            //var users = await _userManager.Users.ToListAsync();
            var role = roles[index];
            if (role == null)
            {
               
                return NotFound();
            }
            for(int i=0; i<model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                }
            }
            return Ok();
        }
        //Testing purpose
        [HttpPost]
        public async Task<IActionResult> CreatRole()
        {
            var role = new ApplicationRole
            {
                Name = "Doctor"
            };
            await _roleManager.CreateAsync(role);
            return Ok();
        }
        //adsadasdadada
        //[HttpPost]
        ////FOR TEST SHOULD NOT BE ANONYMOUS
        //[AllowAnonymous]
        //public async Task<ActionResult> CreateUserRole(UserRoleModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userRole = new ApplicationRole()
        //        {
        //            Name=
        //        }
        //    }
        //}

    }
}
