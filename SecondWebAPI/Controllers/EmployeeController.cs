using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace SecondWebAPI.Controllers
{
   
    public class EmployeeController : ApiController
    {
        // GET api/Employee
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities=new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        // GET api/Employee/5
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
