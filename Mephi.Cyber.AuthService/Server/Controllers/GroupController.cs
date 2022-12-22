// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Mephi.Cyber.AuthService.Core.Models;
// using Microsoft.EntityFrameworkCore;
// using Mephi.Cyber.AuthService.Core;
//
// namespace Mephi.Cyber.AuthService.WebApi.Controllers
// {
//     [Route("admin/group")]
//     public class GroupController : ControllerBase
//     {
//         private readonly AuthRole _authRole;
//         ApplicationContext db;
//         public RoleController(ApplicationContext context)
//         {
//             db = context;
//             _authRole = new  AuthRole(db);
//         }
//
//         [Route("list")]
//         public IActionResult Index(string username)
//         {
//             if (!string.IsNullOrEmpty(username))
//             {
//                 return Ok(_authRole.ListByUser(username));
//             }
//             else
//             {
//                 return Ok(_authRole.List());
//             }
//         }
//
//         [Route("create")]
//         [HttpPost]
//         public IActionResult Create(string name, string parentName, string[] permissions)
//        
//         {
//             if (db.Roles.Any(r => r.Name == name)) {
//                 return BadRequest("Role with such name already exists");
//             }
//             _authRole.Create(name, parentName, permissions);
//
//             return new OkResult();
//         }
//         [Route("delete")]
//         [HttpPost]
//         public IActionResult Delete(string name)
//
//         {
//             if (!db.Roles.Any(r => r.Name == name))
//             {
//                 return BadRequest("Role with such name does not exist");
//             }
//             _authRole.Delete(name);
//             return new OkResult();
//         }
//         
//         [Route("edit/rename")]
//         [HttpPost]
//         public ActionResult Rename(string name, string newName)
//         {
//             if (!db.Roles.Any(r => r.Name == name))
//             {
//                 return BadRequest("Role with such name does not exist");
//             }
//             _authRole.Rename(name, newName);
//             return new OkResult();
//         }
//
//         [Route("edit/add")]
//         [HttpPost]
//         public IActionResult AddPermissions(string roleName, string[] permissions)
//         {
//             if (!db.Roles.Any(r => r.Name == roleName))
//             {
//                 return BadRequest("Role with such name does not exist");
//             }
//             _authRole.AddPermissions(roleName, permissions);
//             return new OkResult();
//         }
//
//         [Route("edit/delete")]
//         [HttpPost]
//         public IActionResult DeletePermissions(string name, string[] permissions)
//         {
//             if (!db.Roles.Any(r => r.Name == name))
//             {
//                 return BadRequest("Role with such name does not exist");
//             }
//
//             _authRole.DeletePermissions(name, permissions);
//             return new OkResult();
//         }
//         [Route("permission")]
//         [HttpGet]
//         public IActionResult Permission(string rolename)
//         {
//             if (!db.Roles.Any(r => r.Name == rolename))
//             {
//                 return BadRequest("Role with such name does not exist");
//             }
//
//             var permissions = _authRole.GetPermissions(rolename);
//             return Ok(permissions);
//         }
//     }
// }
//
