using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{
    

    public class AuthPermission : AuthObject
    {
        public AuthPermission(ApplicationContext context) : base(context)
        {
        }

        public  List<Permission> List()
        {
            return db.Permissions.ToList();
        }

        public List<Permission> List(string roleName)
        {
            var roleId = db.Roles.Single(r => r.Name == roleName).Id;
            var ps  = db.RolePermissions.Where(rp => rp.RoleId==roleId).Select(p=>p.Permission);
            return ps.ToList();
        }
        
        public List<Permission> ListByProject(string projectName)
        {
            var ps = db.Permissions.Where(p => p.Entity.Project.Name == projectName);
            return ps.ToList();
        }

        public void  Create(string name, string objectName, bool read, bool write, bool edit, bool delete)
        {
            Permission permission = new()
            {
                Name = name
            };
            int objectId = db.Entities.Single(o => o.Name == objectName).Id;
            permission.EntityId = objectId;
            permission.Read = read;
            permission.Write = write;
            // permission.Edit = edit;
            // permission.Delete = delete;
            db.Permissions.Add(permission);
            db.SaveChanges();

        }

        public void Delete(string name)
        {
            var permission = db.Permissions.Single(r => r.Name == name);
            db.Permissions.Remove(permission);
            db.SaveChanges();
        }

        public void Rename(string name, string newName)
        {
            var entity = db.Entities.Single(e => e.Name == name);
            entity.Name = newName;
            db.SaveChanges();
        }

        
    }

}

