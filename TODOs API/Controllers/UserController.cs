using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TODOs_API.Data;
using TODOs_API.Models;

namespace TODOs_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoAPIcontext _db;
        private readonly IJsonWebTokenAuthenticationManager _authenticationManager;
        public UserController(TodoAPIcontext db, IJsonWebTokenAuthenticationManager authenticationManager)
        {
            _db = db;
            _authenticationManager = authenticationManager;
        }

        //api/User/Register
        [HttpPost("Register")]
        public ActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Model validation failed" });
            }
            User duplicateUser = _db.Users.Where(q => q.Username == user.Username).FirstOrDefault();
            if (duplicateUser != null)
            {
                return BadRequest(new { message = "User with this username already exists" });
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok(new { message = "User Registered" });
        }

        //api/User/Login
        [HttpGet("Login")]
        public ActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Model validation failed" });
            }
            User loginUser = _db.Users.Where(q => q.Username == user.Username).FirstOrDefault();
            if (loginUser == null)
            {
                return BadRequest(new { message = "Invalid Email or Password" });
            }
            bool passwordComparison = BCrypt.Net.BCrypt.Verify(user.Password, loginUser.Password);
            if (!passwordComparison)
            {
                return BadRequest(new { message = "Invalid Email or Password" });
            }
            string jsonWebToken = _authenticationManager.Authenticate(user.Username);
            return Ok(new { token = jsonWebToken });
        }
    }
}
