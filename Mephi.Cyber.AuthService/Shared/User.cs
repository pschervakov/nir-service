using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{
    

    public class AuthUser : AuthObject
    {
        public AuthUser(ApplicationContext context) : base(context)
        {
        }

        public  List<User> List()
        {
            return db.Users.ToList<User>();
        }
        
        public List<object> ListApi() 
        {
            return new List<object> { db.Users.Select(u => new { u.Id, u.Name, u.Email }).ToList() };
        }


        public string  Create(string username, string password, string[] roles)
        {
            User newUser = new()
            {
                Name = username
            };
            
            int salt = Password.CreateRandomSalt();
            Password pwd = new Password(password, salt);
            string strHashedPassword = pwd.ComputeSaltedHash();
            newUser.Salt = salt;
            newUser.SaltedHash = strHashedPassword;
            db.Users.Add(newUser);
            db.SaveChanges();
            var user = db.Users.Single(u => u.Name == username);
            if (roles.Length > 0)
            {
                foreach (string roleName in roles)
                {
                    var role = db.Roles.Single(r => r.Name == roleName);

                    RoleBinding roleBinding = new()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    db.RoleBindings.Add(roleBinding);
                }
                db.SaveChanges();
               
            }
           
            return password;
        }
        
        public void AddRoles(string username, string[] roles)
        {
            if (roles.Length > 0)
            {   
                var user = db.Users.Single(u => u.Name == username);
                foreach (string roleName in roles)
                {
                    var role = db.Roles.Single(r => r.Name == roleName);

                    RoleBinding roleBinding = new()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    db.RoleBindings.Add(roleBinding);
                }

                db.SaveChanges();
            }
        }
        
        public void DeleteRole(string username, string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var rb = db.RoleBindings.Single(rb => (rb.User.Name == username && rb.Role.Name == roleName));

                db.RoleBindings.Remove(rb);
                
                db.SaveChanges();
            }

        }
        public void Delete(string name)
        {
            var user = db.Users.Single(u => u.Name == name);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public void Rename(string name, string newName)
        {
            var user = db.Users.Single(u => u.Name == name);
            user.Name = newName;
            db.SaveChanges();
        }

        public List<Role>  GetRoles(string name)
        {
            var roles = db.RoleBindings.Where(rb => rb.User.Name==name).Select(rb=>rb.Role);
            return roles.ToList();

        }
        
         public void WriteRelation()
        {
            var parser = new RelationParser();
            // FIXME this should be an argument
            var inputs = new List<string>
            {
                // Actions, relations, and assignments can be defined in any order
                "DEFINE ACTION read",
                "DEFINE ACTION write",
                "ASSIGN User123 AS Editor TO Document456",
                "DEFINE ACTION delete",
                "DEFINE ACTION approve",
                "DEFINE RELATION Editor { PERMITS: read, PERMITS: write, PERMITS: approve }",
            };

            var actions = parser.ParseActions(inputs);
            var relations = parser.ParseRelations(inputs, actions);
            var assignments = parser.ParseAssignments(inputs, relations);
            
            foreach (var action in actions.Values)
            {
                var existingAct = db.Acts.FirstOrDefault(a => a.Name == action.Name);
                if (existingAct == null)
                {
                    db.Acts.Add(new Act { Name = action.Name });
                   
                }
            }
            db.SaveChanges();

            
            foreach (var relation in relations.Values)
            {
                var relationEntity = new Relation { Name = relation.Name };
                // FIXME add handler if exists
                db.Relations.Add(relationEntity);
                // FIXME don't save on each iteration
                db.SaveChanges();
                foreach (var actionName in relation.PermittedActions)
                {
                    var actionEntity = db.Acts.Single(a => a.Name == actionName);
                    RelationAct relationAct = new()
                    {
                        ActId = actionEntity.Id,
                        RelationId = relationEntity.Id
                    };
                    db.RelationActs.Add(relationAct);
                }
               
            }
            db.SaveChanges();

            // Save assignments
            foreach (var assignment in assignments)
            {
                var relationEntity = db.Relations.Single(r => r.Name == assignment.RelationName);
                var user = db.Users.Single(u => u.Name == assignment.Subject);
                var entity = db.Entities.Single(e => e.Name == assignment.Object);
                db.Assignments.Add(new Assignment
                {
                    UserId = user.Id,
                    RelationId = relationEntity.Id,
                    EntityId = entity.Id
                });
            }
            db.SaveChanges();
        }

    }

}

