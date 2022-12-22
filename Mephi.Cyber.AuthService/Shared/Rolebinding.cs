using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{
    

    public class AuthRoleBinding : AuthObject
    {
        public AuthRoleBinding(ApplicationContext context) : base(context)
        {
        }
        public List<RoleBinding> List()
        {
            return db.RoleBindings.ToList();
        }
        public void Create(string userName, string roleName)
        {
            var role = db.Roles.Single(r => r.Name == roleName);
            var user = db.Users.Single(u => u.Name == userName);
            RoleBinding roleBinding = new()
            {
                UserId = user.Id,
                RoleId = role.Id
            };

            db.SaveChanges();

           
        }
        
        public void DeleteMultiple(string userName, string[] roles)
        {
            var user = db.Users.Single(u => u.Name == userName);
            foreach (string roleName in roles)
            {
                var role = db.Roles.Single(r => r.Name == roleName);
                var rb = db.RoleBindings.Single(rb => rb.RoleId == role.Id && rb.UserId == user.Id);
                db.RoleBindings.Remove(rb);
            }
            db.SaveChanges();
            
        }
    }

}

