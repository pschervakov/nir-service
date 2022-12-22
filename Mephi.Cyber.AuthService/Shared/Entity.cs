using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{
    
    public class AuthEntity : AuthObject
    {
        public AuthEntity(ApplicationContext context) : base(context)
        {
        }

        public  List<Entity> List()
        {
            return db.Entities.ToList();
        }

        public List<Entity> List(string projectName)
        {
            var entities = db.Entities.Where(e => e.Project.Name == projectName);
            return entities.ToList();
        }

        public static  List<Entity> GetParents(string entityName, string projectName)
        {
            var reqEntity = db.Entities.Single(o => o.Project.Name == projectName && o.Name == entityName);
            var curEntity = reqEntity;
            List<Entity> parentEntities = new();
            parentEntities.Add(curEntity);
            Console.WriteLine("lala");
            Console.WriteLine(curEntity.ParentId);
            while (curEntity.ParentId is not null)
            {
                curEntity = curEntity.Parent;
                parentEntities.Add(curEntity);
            }
            return parentEntities;
        }


        public void Create(string name, string parentName, string projectName,  string ownerName)
        {
            Entity entity = new()
            {
                Name = name
            };
            
            // Relation relation = new()
            // {
            //     Name = "owner"
            // };

            if (!string.IsNullOrEmpty(parentName))
            {
                var parentEntity = db.Entities.Single(e => e.Name == parentName);
                entity.Parent = parentEntity;
            }
            
            if (!string.IsNullOrEmpty(ownerName))
            {
                var user = db.Users.Single(u => u.Name == ownerName);
                // relation.UserId = user.Id;
            }
           
            var project = db.Projects.Single(p => p.Name == projectName);
            
            entity.Project = project;
            // relation.Entity = entity;
            db.Entities.Add(entity);
            // db.Relations.Add(relation);
            db.SaveChanges();

        }

        public void Delete(string name)
        {
            var entity = db.Entities.Single(p => p.Name == name);
            var childs = db.Entities.Where(e => e.ParentId == entity.Id);
            foreach(var child in childs)
            {
                child.Parent = entity.Parent;
            }
            db.Entities.Remove(entity);
            db.SaveChanges();
        }

        public void Rename(string name, string newName)
        {
            var entity = db.Entities.Single(p => p.Name == name);
            entity.Name = newName;
            db.SaveChanges();
        }

    }


}

