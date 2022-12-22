using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core;

namespace Mephi.Cyber.AuthService.WebApi.Controllers
{
    [Route("admin/permission")]
    public class PermissionController : Controller
    {
        private readonly AuthPermission _authPermission;
        readonly ApplicationContext db;

        public PermissionController(ApplicationContext context)
        {
            db = context;
            _authPermission = new AuthPermission(db);
        }

        [Route("list")]
        public IActionResult Index(string roleName, string projectName)
        {
            if (!string.IsNullOrEmpty(projectName))
            {
                return Ok(_authPermission.ListByProject(projectName));
            }
            else if (!string.IsNullOrEmpty(roleName))
            {
                return Ok(_authPermission.List(roleName));
            }
            else
            {
                return Ok(_authPermission.List());
            }
        }


        [Route("create")]
        [HttpPost]
        public IActionResult Create(string name, string objectName, bool read, bool write)
        {
            if (db.Permissions.Any(r => r.Name == name))
            {
                return BadRequest("Permission with such name already exists");
            }

            if (!db.Entities.Any(o => o.Name == objectName))
            {
                return BadRequest("Object with such name does not exist");
            }

            _authPermission.Create(name, objectName, read, write, write, write);
            return new OkResult();
        }

        [Route("delete")]
        [HttpPost]
        public IActionResult Delete(string name)
        {
            if (!db.Permissions.Any(r => r.Name == name))
            {
                return BadRequest("Permission with such name does not exist");
            }

            _authPermission.Delete(name);
            return new OkResult();
        }

        [Route("rename")]
        [HttpPost]
        public IActionResult Rename(string name, string newName)
        {
            if (!db.Permissions.Any(r => r.Name == name))
            {
                return BadRequest("Permission with such name does not exist");
            }

            _authPermission.Rename(name, newName);
            return new OkResult();
        }
    }
}