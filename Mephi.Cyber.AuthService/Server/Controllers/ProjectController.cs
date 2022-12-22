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
    [Route("admin/project")]
    public class ProjectController : Controller
    {
        readonly ApplicationContext db;
        private readonly AuthProject _authProject;
        public ProjectController(ApplicationContext context)
        {
            db = context;
            _authProject = new AuthProject(db);
        }
        [Route("list")]
        public IActionResult Index()
        {
            return Ok(_authProject.List());
        }

        [Route("create")]
        [HttpPost]
        public IActionResult Create(string name)
        {
            if (db.Projects.Any(r => r.Name == name))
            {
                return BadRequest("Project with such name already exists");
            }
            var secrets = _authProject.Create(name);
            return Ok(secrets);
            
        }
        [Route("delete")]
        [HttpPost]
        public ActionResult Delete(string name)
        {
            if (!db.Projects.Any(r => r.Name == name))
            {
                return BadRequest("Project with such name does not exist");
            }
            _authProject.Delete(name);
            return new OkResult();
        }
        [Route("rename")]
        [HttpPost]
        public IActionResult Rename(string name, string newName)
        {
            if (!db.Projects.Any(r => r.Name == name))
            {
                return BadRequest("Project with such name does not exist");
            }
            _authProject.Rename(name, newName);
            return new OkResult();
        }

        [Route("regenerate")]
        [HttpPost]
        public IActionResult Regenerate(string name)
        {
            if (!db.Projects.Any(r => r.Name == name))
            {
                return BadRequest("Project with such name does not exist");
            }
            var secret = _authProject.Regenerate(name);
            return Ok(secret);

        }
    }
}

