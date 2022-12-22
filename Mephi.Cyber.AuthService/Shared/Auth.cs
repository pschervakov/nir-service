using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{


    public class Auth : AuthObject
    {
        public Auth(ApplicationContext context) : base(context)
        {
        }

        public bool AllowedToLogin(string username, string password)
        {
            var user = db.Users.Single(u => u.Name == username);
            var saltFromDb = user.Salt;
            var storedSaltedHash = user.SaltedHash;
            Password pwd = new(password, saltFromDb);

            if (pwd.ComputeSaltedHash() == storedSaltedHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
       
        
        
        // public bool IsOwner(string username, string objectName)
        // {
        //     var user = db.Users.Single(u => u.Name == username);
        //     var relation = db.Relations.Any(r =>
        //         r.User.Name == username && r.Entity.Name == objectName && r.Name == "owner");
        //
        //     return relation;
        // }
        //
        public List<Permission> GetPermissions(string username, string projectName)
        {
            var user = db.Users.Single(u => u.Name == username);
            var project = db.Projects.Single(p => p.Name == projectName);
            var roleIds = db.RoleBindings.Where(rb => rb.UserId == user.Id).Select(rb => rb.RoleId).ToHashSet();

            roleIds = AuthRole.GetParentRoles(roleIds);

            var perIds = db.RolePermissions.Where(rp => roleIds.Contains(rp.RoleId)).Select(rp => rp.PermissionId);
            var permissions = db.Permissions.Where(p => perIds.Contains(p.Id) && p.Entity.ProjectId == project.Id).ToList();

            return permissions;
        }

        public bool CheckPermissions(string username, string projectName, string entityName, string action)
        {
            var res = false;
            var parentEntities = AuthEntity.GetParents(entityName, projectName);

            var user = db.Users.Single(u => u.Name == username);
            var project = db.Projects.Single(p => p.Name == projectName);

            var groups = db.UserGroups.Where(ug => ug.UserId == user.Id).Select(g=>g.Id);
            
            var roleIds = db.RoleBindings.Where(rb => rb.UserId == user.Id).Select(rb => rb.RoleId).ToHashSet();
            var groupRoleIds = db.RoleGroups.Where(rg => groups.Contains(rg.GroupId)).Select(rg => rg.RoleId).ToHashSet();
            roleIds.UnionWith(groupRoleIds);
            roleIds = AuthRole.GetParentRoles(roleIds);
            
            var perIds = db.RolePermissions.Where(rp => roleIds.Contains(rp.RoleId)).Select(rp => rp.PermissionId);
            
            IQueryable<Permission> permissions;
            
            if  (action == "read")
            {
                permissions = db.Permissions.Where(p => perIds.Contains(p.Id) &&  p.Read  && p.Entity.ProjectId == project.Id);
            }
            else if (action == "write")
            {
                permissions = db.Permissions.Where(p => perIds.Contains(p.Id) &&  p.Write  && p.Entity.ProjectId == project.Id);
            }
            else
            {
                return false;
            }
            
            var entities = permissions.Select(p => p.Entity);

            foreach (var pe in parentEntities)
            {
                if (entities.Contains(pe))
                {
                    res = true;
                }
            }
            // if (IsOwner(username, entityName))
            // {
            //     res = true;
            // }
            
            
            return res;

        }

    }

}

