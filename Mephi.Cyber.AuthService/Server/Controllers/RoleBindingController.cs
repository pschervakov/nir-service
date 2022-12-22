using Microsoft.AspNetCore.Mvc;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core;

namespace Mephi.Cyber.AuthService.WebApi.Controllers
{
    [Route("admin/rolebinding")]
    public class RoleBindingController : Controller
    {
        private readonly AuthRoleBinding _authrolebing;
        ApplicationContext db;
        public RoleBindingController(ApplicationContext context)
        {
            db = context;
            _authrolebing = new AuthRoleBinding(db);
        }
        
        [Route("create")]
        [HttpPost]
        public IActionResult Create(string userName, string roleName)
        {
            _authrolebing.Create(userName, roleName);

            return new OkResult();
        }
        [Route("edit/delete")]
        [HttpPost]
        public IActionResult DeleteMultiple(string userName, string[] roles)
        {
            _authrolebing.DeleteMultiple(userName, roles);
            return new OkResult();
        }
    }
}
