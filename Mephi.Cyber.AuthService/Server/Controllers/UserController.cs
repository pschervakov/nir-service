using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;
using Mephi.Cyber.AuthService.Core;

namespace Mephi.Cyber.AuthService.WebApi.Controllers
{
    [Route("admin/user")]
    public class UserController : ControllerBase
    {
        private readonly AuthUser _authUser;
        ApplicationContext db;
        public UserController(ApplicationContext context)
        {
            db = context;
            _authUser = new AuthUser(db);
        }

        [Route("list")]
        public IActionResult Index()
        {
            return Ok(_authUser.List());
        }
        
        // FIXME
        [Route("/api/user/list")]
        public IActionResult IndexApi()
        {
            return Ok(_authUser.ListApi());
        }
        
        [Route("create")]
        [HttpPost]
        public IActionResult Create(string username, string password,  string[] roles)
        {
            if (db.Users.Any(r => r.Name == username))
            {
                return BadRequest("User with such name already exists");
            }
           
            var pass = _authUser.Create(username, password, roles);

            return Ok(pass);
            
        }
        [Route("delete")]
        [HttpPost]
        public IActionResult Delete(string name)
        {
            if (!db.Users.Any(r => r.Name == name))
            {
                return BadRequest("User with such name does not exist");
            }
            _authUser.Delete(name);
            return new OkResult();
        }

        [Route("rename")]
        [HttpPost]
        public IActionResult Rename(string name, string newName)
        {
            if (!db.Users.Any(r => r.Name == name))
            {
                return BadRequest("User with such name does not exist");
            }
            _authUser.Rename(name, newName);
            return new OkResult();
        }
        [Route("add_roles")]
        [HttpPost]
        public IActionResult AddRoles(string username, string[] roles)
        {
            if (!db.Users.Any(r => r.Name == username))
            {
                return BadRequest("User with such name does not exist");
            }
            _authUser.AddRoles(username, roles);
            return new OkResult();
        }
        [Route("delete_role")]
        public IActionResult DeleteRole(string username, string roleName )
        {
            if (!db.Users.Any(r => r.Name == username))
            {
                return BadRequest("User with such name does not exist");
            }

            _authUser.DeleteRole(username, roleName);
            return new OkResult();
        }
        
        [Route("roles")]
        [HttpGet]
        public IActionResult Role(string username)
        {
            if (!db.Users.Any(r => r.Name == username))
            {
                return BadRequest("User with such name does not exist");
            }

            var roles = _authUser.GetRoles(username); 

            return Ok(roles);
        }
    }
}

