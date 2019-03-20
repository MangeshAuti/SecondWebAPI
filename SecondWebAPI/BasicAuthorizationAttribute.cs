using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SecondWebAPI
{
    public class EmployeeSecurity
    {
        public static bool Login(string username, string password, out string role)
        {
            role = string.Empty;
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Users.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
                if (entity == null)
                {
                    return false;
                }
                else
                {
                    role = entity.Role;
                    return true;
                }
            }
            //using (EmployeeDBEntities entities = new EmployeeDBEntities())
            //{

            //    return entities.Users.Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            //}
        }
    }
    public class BasicAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                    string[] userPassArray = decodedAuthenticationToken.Split(':');
                    string username = userPassArray[0];
                    string password = userPassArray[1];
                    string role = "";
                    string[] roles = new string[1];

                    if (EmployeeSecurity.Login(username, password, out role))
                    {
                        if (string.IsNullOrEmpty(role))
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                        else
                        {
                            roles[0] = role;
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), roles);
                        }
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch(Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}