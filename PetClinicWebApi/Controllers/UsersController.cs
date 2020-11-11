﻿using System;
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
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, PetService petService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _petService = petService;
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
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
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
        public async Task<ActionResult> DeleteUser(string email)
        {
            if(email != null)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    await _userManager.DeleteAsync(user);
                    return Ok();

                }
                catch(Exception ex)
                {

                }
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
