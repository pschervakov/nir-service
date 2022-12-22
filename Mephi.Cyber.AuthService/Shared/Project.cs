using System;
using System.Collections.Generic;
using System.Linq;
using Mephi.Cyber.AuthService.Core.Models;
using Mephi.Cyber.AuthService.Core.Utils;

namespace Mephi.Cyber.AuthService.Core
{
    public class AuthObject
    {
        public static ApplicationContext db;
        public AuthObject(ApplicationContext context)
        {
            db = context;
        }
        
    }

    public class AuthProject : AuthObject
    {
        public AuthProject(ApplicationContext context) : base(context)
        {
        }

        public List<Project> List()
        {
            return db.Projects.ToList();
        }


        public Dictionary<string, string> Create(string name)
        {
            Project project = new()
            {
                Name = name
            };
            var sharedSecret = Token.GenerateSharedSecret();
            var authSecret = Token.GenerateAuthSecret();
            project.SharedSecret = sharedSecret;
            project.AuthSecret = authSecret;
            db.Projects.Add(project);
            db.SaveChanges();

            var secrets = new Dictionary<string, string>(){
                {"sharedSecret", sharedSecret},
                {"authSecret", authSecret}
            };

            return secrets;
        }

        public void Delete(string name)
        {
            var project = db.Projects.Single(p => p.Name == name);
            db.Projects.Remove(project);
            db.SaveChanges();
        }

        public void Rename(string name, string newName)
        {
            var project = db.Projects.Single(p => p.Name == name);
            project.Name = newName;
            db.SaveChanges();
        }

        public string Regenerate(string name)
        {
            var project = db.Projects.Single(p => p.Name == name);
            var secret = Token.GenerateSharedSecret();
            project.SharedSecret = secret;
            db.SaveChanges();

            return secret;
        }

    }


}

