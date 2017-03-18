using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace BasketballTinder.Controllers
{
    public class WydarzeniaController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Entities().Wydarzenia.ToList(), JsonMediaTypeFormatter.DefaultMediaType);
        }

        public HttpResponseMessage Post(Wydarzenie wydarzenie)
        {
            List<string> errors = new List<string>();
            if (wydarzenie.Data == null)
                errors.Add("Brak daty");
            if (wydarzenie.Lokalizacja == null || wydarzenie.Lokalizacja == "")
                errors.Add("Brak lokalizacji");
            if (errors.Count > 0)
            {
                string message = "";
                errors.ForEach(x => { message += x + " "; });
                return Request.CreateResponse(HttpStatusCode.BadRequest, message, JsonMediaTypeFormatter.DefaultMediaType);
            }
            else
            {
                try
                {
                    Entities db = new Entities();
                    wydarzenie.Id = Guid.NewGuid().ToString();
                    db.Entry(wydarzenie).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "", JsonMediaTypeFormatter.DefaultMediaType);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
                }
            }
        }

        public HttpResponseMessage Delete(string id)
        {
            try
            {
                Entities db = new Entities();
                Wydarzenie wydarzenie = db.Wydarzenia.Where(x => x.Id == id).First();
                db.Entry(wydarzenie).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "", JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Wydarzenie o podanym Id nie istnieje", JsonMediaTypeFormatter.DefaultMediaType);
            }
        }

        public HttpResponseMessage Put(Wydarzenie wydarzenie)
        {
            List<string> errors = new List<string>();
            if (wydarzenie.Data == null)
                errors.Add("Brak daty");
            if (wydarzenie.Lokalizacja == null || wydarzenie.Lokalizacja == "")
                errors.Add("Brak lokalizacji");
            if (errors.Count > 0)
            {
                string message = "";
                errors.ForEach(x => { message += x + " "; });
                return Request.CreateResponse(HttpStatusCode.BadRequest, message, JsonMediaTypeFormatter.DefaultMediaType);
            }
            else
            {
                try
                {
                    Entities db = new Entities();
                    db.Entry(wydarzenie).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "", JsonMediaTypeFormatter.DefaultMediaType);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
                }
            }
        }
    }
}
