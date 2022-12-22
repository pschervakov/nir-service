using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.WebApi.Controllers
{
    [Route("admin/object")]
    public class EntityController : Controller
    {
        readonly ApplicationContext db;
        private readonly AuthEntity _authEntity;
        public EntityController(ApplicationContext context)
        {
            db = context;
            _authEntity = new AuthEntity(db);
        }
        [Route("list")]
        public IActionResult Index(string projectName)
        {
            if (!string.IsNullOrEmpty(projectName))
            {
                return Ok(_authEntity.List(projectName));
            } else
            {
                return Ok(_authEntity.List());
            }
            
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create(string name, string parentName, string projectName, string ownerName )
        {
            if (db.Entities.Any(r => r.Name == name))
            {
                return BadRequest("Object with such name already exists");
            }
            _authEntity.Create(name, parentName, projectName, ownerName);
            return new OkResult(); 
            
        }
        [Route("delete")]
        [HttpPost]
        public ActionResult Delete(string name)
        {
            if (!db.Entities.Any(r => r.Name == name))
            {
                return BadRequest("Object with such name does not exist");
            }
            _authEntity.Delete(name);
            return new OkResult();
        }
        [Route("rename")]
        [HttpPost]
        public IActionResult Rename(string name, string newName)
        {
            if (!db.Projects.Any(r => r.Name == name))
            {
                return BadRequest("Object with such name does not exist");
            }
            _authEntity.Rename(name, newName);
            return new OkResult();
        }
        
    }
}

