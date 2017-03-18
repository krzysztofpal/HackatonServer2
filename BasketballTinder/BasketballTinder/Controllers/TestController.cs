using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BasketballTinder.Controllers
{
    public class TestController : ApiController
    {
        public List<string> Get()
        {
            Entities entities = new Entities();
            entities.Entry(new MyEntity() { Name = new Random().NextDouble().ToString() }).State = System.Data.Entity.EntityState.Added;
            entities.SaveChanges();
            return entities.MyEntities.Select(x => x.Name).ToList();
        }
    }
}
