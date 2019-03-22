using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using EmployeeDataAccess;

namespace SecondWebAPI.Controllers
{

    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        [Authorize]
        // GET api/Employee
        //IHttpActionResult webapi2 system.web.http
        public IHttpActionResult Get()
        {
            //if (Thread.CurrentPrincipal.IsInRole("Admin"))
            //{
                using (EmployeeDBEntities entities=new EmployeeDBEntities())
                {
                    return Ok(entities.Employees.ToList());
                    //return entities.Employees.ToList();
                }
            //}
            //else
            //{
              //  return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User is not admin");
            //}
        }

        // GET api/Employee/5
        [Route("{id:int:min(0)}")]
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity=entities.Employees.FirstOrDefault(e => e.Id==id);
                if(entity==null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee details not found for Id =" + id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
        }

        // POST api/Employee
        public HttpResponseMessage Post([FromBody]Employee emp)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(emp);
                    entities.SaveChanges();
                    var message=Request.CreateResponse(HttpStatusCode.Created,emp);
                    message.Headers.Location = new Uri(Request.RequestUri + emp.Id.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT api/Employee/5
        public HttpResponseMessage Put(int id, [FromBody]Employee emp)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee details not found for update Id =" + id);
                    }
                    else
                    {
                        entity.FirstName = emp.FirstName;
                        entity.LastName = emp.LastName;
                        entity.Gender = emp.Gender;
                        entity.Salary = emp.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/Employee/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.Id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee details not found for delete Id =" + id);
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
