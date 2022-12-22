using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mephi.Cyber.AuthService.Core.Models;
using System.Security.Claims;
using Mephi.Cyber.AuthService.Core.Utils;
using Mephi.Cyber.AuthService.Core;
using System.Net;
using System.Xml.Serialization;
namespace Mephi.Cyber.AuthService.WebApi.Controllers    
{
    public class AuthController : ControllerBase
    {
        private readonly Auth _auth;
        private readonly AuthUser _authUser;
        readonly ApplicationContext db;
        public AuthController(ApplicationContext context)
        {
            db = context;
            _auth = new Auth(db);
            _authUser = new AuthUser(db);

        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password, string projectName)
        {
            if (!db.Projects.Any(p => p.Name == projectName))
            {
                return BadRequest("Project with such name does not exist");
            }
            if (!db.Users.Any(u => u.Name == username))
            {
                return Unauthorized("User with such name does not exist");
            }
            
            if (_auth.AllowedToLogin(username,password))
            {
                var secret = db.Projects.Single(p => p.Name == projectName).SharedSecret;
                var user = db.Users.Single(u => u.Name == username);
                var token = Token.GenerateToken(username, secret);
                return Ok(token);
                
            }
            else
            {
                return Unauthorized("Incorrect password"!);
            }
        }
       
        [Route("user_permissions")]
        [HttpGet]
        public IActionResult GetPermission(string username, string projectName)
        {
            
            if (!db.Projects.Any(p => p.Name == projectName))
            {
                return BadRequest("Project with such name does not exist");
            }
            
            if (!db.Users.Any(u => u.Name == username))
            {
                return BadRequest("User with such name does not exist");
            }
            
            
            var authToken = HttpContext.Request.Headers["Authorization"];
            var realToken = db.Projects.Single(p => p.Name == projectName).AuthSecret;
            if (authToken != realToken)
            {
                return Unauthorized("Unauthorized");
            }
            var permissions = _auth.GetPermissions(username, projectName);
            return Ok(permissions);
        }

        [Route("mephi_login")]
        [HttpGet]
        public async Task<ActionResult> MephiSignIn(string ticket, string redirectUri, string projectName)
        {   
            if (!db.Projects.Any(p => p.Name == projectName))
            {
                return BadRequest("Project with such name does not exist");
            }
            
            var url = $"{Request.Scheme}://{Request.Host}{Request.Path}?redirectUri={redirectUri}&projectName={projectName}";
            
            var client = new HttpClient();
            
            // FIXME should be configuration parameter
            var uri = new Uri($"https://auth.mephi.ru/serviceValidate?ticket={ticket}&service={WebUtility.UrlEncode(url)}");
            var response = await client.GetAsync(uri);
            var textResult = await response.Content.ReadAsStreamAsync();

            XmlSerializer serializer = new XmlSerializer(typeof(serviceResponse));

            serviceResponse serviceResponse = (serviceResponse)serializer.Deserialize(textResult);

            var success = !string.IsNullOrEmpty(serviceResponse?.authenticationSuccess?.user);
            
            if (success)
            {
                string username = serviceResponse.authenticationSuccess.user;
                if (!db.Users.Any(r => r.Name == username))
                {
                    // FIXME
                     var  password = RandomPasswordGenerator.GeneratePassword(passwordSize: 15);
                    _authUser.Create(username, password, new string[] {});
                }
                var user = db.Users.Single(u => u.Name == username);
                var secret = db.Projects.Single(p => p.Name == projectName).SharedSecret;
                var token = Token.GenerateToken(user.Name, secret);
                
                return RedirectPermanent($"{redirectUri}/?token={token}");
            }

            return Unauthorized("Unauthorized");
        }

        [Route("oidc_login")]
        [HttpGet]
        public async Task<ActionResult>  Oidc(string id_token, string redirectUri, string projectName)
        {
            var key = "";
            var success = Token.ValidateRsaCurrentToken(id_token, key);
            if (success)
            {
               var  username = Token.GetClaim(id_token, "preferred_username");

                if (!db.Users.Any(r => r.Name == username))
                {   
                    // FIXME
                    var  password = RandomPasswordGenerator.GeneratePassword(passwordSize: 15);
                    _authUser.Create(username, password, new string[] {});
                }
                var user = db.Users.Single(u => u.Name == username);
                var secret = db.Projects.Single(p => p.Name == projectName).SharedSecret;
                var token = Token.GenerateToken(username, secret);
                return RedirectPermanent($"https://{redirectUri}/?token={token}");
            }



            return Unauthorized("Unauthorized");
        }

        [Route("check_user_permission")]
        [HttpGet]
        public IActionResult CheckPermission(string username, string projectName, string entityName, string actionName)
        {
            var res = false;
            
            if (!db.Projects.Any(p => p.Name == projectName))
            {
                return BadRequest("Project with such name does not exist");
            }
            
            if (!db.Users.Any(u => u.Name == username))
            {
                return BadRequest("User with such name does not exist");
            }
            
            if (!db.Entities.Any(e => e.Name == entityName))
            {
                return BadRequest("Object with such name does not exist");
            }
            
            var authToken = HttpContext.Request.Headers["Authorization"];
            var realToken = db.Projects.Single(p => p.Name == projectName).AuthSecret;
            if (authToken != realToken)
            {
                return Unauthorized("Unauthorized");
            }

            res = _auth.CheckPermissions(username, projectName, entityName, actionName);

            return Ok(res);
        }
        //[Route("valid")]
        //[HttpGet]
        //public IActionResult Valid(string token)
        //{
        //    //string s = "";
        //    var secret = db.Projects.Single(p => p.Name == "test2").SharedSecret;
        //    if (!Token.ValidateCurrentToken(token, secret))
        //    {
        //        return Content("Failed");
        //    }
        //    var user = Token.GetClaim(token, "userId");

        //    return Content(user);
        //}

    }
}

