using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace BasketballTinder.Controllers
{
    public class LogowanieController : ApiController
    {
        public HttpResponseMessage Post(LoginPostModel model)
        {
            Entities entities = new Entities();
            Koszykarz koszykarz = entities.Koszykarze.Where(x => x.Login == model.Login && x.Haslo == model.Haslo).FirstOrDefault();
            if (koszykarz == null)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            UserToken token = new UserToken();
            token.ExpireDate = DateTime.Now.AddHours(1);
            token.Id = Guid.NewGuid().ToString();
            token.Token = Guid.NewGuid().ToString();
            token.Login = model.Login;
            entities.Entry(token).State = System.Data.Entity.EntityState.Added;
            entities.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, token, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }

    public class LoginPostModel
    {
        public string Login { get; set; }
        public string Haslo { get; set; }
    }
}
