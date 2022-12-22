using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{
    

    public class AuthRole : AuthObject
    {
        public AuthRole(ApplicationContext context) : base(context)
        {
        }

        public  List<Role> List()
        {
            return db.Roles.ToList();
        }
        
        public  List<Role> ListByUser(string username)
        {
            var roles = db.RoleBindings.Where(rb => rb.User.Name==username).Select(rb=>rb.Role);
            return roles.ToList();
        }


        public void  Create(string name, string parentName, string[] permissions)
        {
            Role newRole = new()
            {
                Name = name
            };

            db.Roles.Add(newRole);
            db.SaveChanges();
            var role = db.Roles.Single(r => r.Name == name);
            
            foreach (string perName in permissions)
            {
                var per = db.Permissions.Single(p => p.Name == perName);
                db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = per.Id });
            }
            if (!string.IsNullOrEmpty(parentName))
            {
                var parentRole = db.Roles.Single(r => r.Name == parentName);
                db.RoleParents.Add(new RoleParent { RoleId = role.Id, ParentId = parentRole.Id });
            }
            
            db.SaveChanges();

        }

        public void Delete(string name)
        {
            var role = db.Roles.Single(r => r.Name == name);
            db.Roles.Remove(role);
            db.SaveChanges();
        }

        public void Rename(string name, string newName)
        {
            var role = db.Roles.Single(r => r.Name == name);
            role.Name = newName;
            db.SaveChanges();
        }

        public void AddPermissions(string name, string[] permissions)
        {
            var role = db.Roles.Single(r => r.Name == name);
            foreach (string perName in permissions)
            {
                var per = db.Permissions.Single(p => p.Name == perName);
                db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = per.Id });
            }
            db.SaveChanges();

        }

        public void DeletePermissions(string name, string[] permissions)
        {
            var role = db.Roles.Single(r => r.Name == name);
            foreach (string perName in permissions)
            {
                var per = db.Permissions.Single(p => p.Name == perName);
                var rp = db.RolePermissions.Single(rp => rp.RoleId == role.Id && rp.PermissionId == per.Id);
                db.RolePermissions.Remove(rp);
            }
            db.SaveChanges();

        }

        public static HashSet<int> GetParentRoles(HashSet<int> roleIds)
        {
            var diff = roleIds.Count;
            while (diff != 0)
            {
                var current = roleIds.Count;
                var parentRoles = db.RoleParents.Where(rp => roleIds.Contains(rp.RoleId)).Select(r => r.Id).ToHashSet();
                roleIds.UnionWith(parentRoles);
                diff = roleIds.Count - current;
            }

            return roleIds;
        }

        public List<Permission> GetPermissions(string name)
        {
            var roleIds = db.Roles.Where(r => r.Name == name).Select(r => r.Id).ToHashSet();
            roleIds = GetParentRoles(roleIds);

            var perIds = db.RolePermissions.Where(rp => roleIds.Contains(rp.RoleId)).Select(rp => rp.PermissionId);
            var permissions = db.Permissions.Where(p => perIds.Contains(p.Id)).ToList();

            return permissions;
        }
    }

}

