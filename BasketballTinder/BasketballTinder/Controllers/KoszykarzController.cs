﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace BasketballTinder.Controllers
{
    
    public class KoszykarzController : ApiController
    {
        [BasicAuthenticationAtrybut]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Entities().Koszykarze.ToList(), JsonMediaTypeFormatter.DefaultMediaType);
        }

        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Post(Koszykarz koszykarz)
        {
            List<string> errors = new List<string>();
            if (koszykarz.Haslo == null || koszykarz.Haslo == "")
                errors.Add("Brak hasła");
            if (koszykarz.Imie == null || koszykarz.Imie == "")
                errors.Add("Brak imienia");
            if (koszykarz.Login == null || koszykarz.Login == "")
                errors.Add("Brak loginu");
            if (koszykarz.Nazwisko == null || koszykarz.Nazwisko == "")
                errors.Add("Brak nazwiska");
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
                    if (db.Koszykarze.Select(x => x.Login).ToList().Exists(x=>x == koszykarz.Login))
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Login jest już zajęty", JsonMediaTypeFormatter.DefaultMediaType);
                    koszykarz.Id = Guid.NewGuid().ToString();
                    db.Entry(koszykarz).State = System.Data.Entity.EntityState.Added;

                    UserToken token = new UserToken();
                    token.Id = Guid.NewGuid().ToString();
                    token.Token = Guid.NewGuid().ToString();
                    token.Login = koszykarz.Login;
                    token.ExpireDate = DateTime.Now.AddHours(1);
                    db.Entry(token).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, token, JsonMediaTypeFormatter.DefaultMediaType);
                }
                catch(Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");
                }
            }
        }
        [BasicAuthenticationAtrybut]
        [System.Web.Mvc.HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            try
            {
                Entities db = new Entities();
                Koszykarz koszyakrz = db.Koszykarze.Where(x => x.Id == id).First();
                db.Entry(koszyakrz).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "", JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch(Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Koszykarz o podanym Id nie istnieje", JsonMediaTypeFormatter.DefaultMediaType);
            }
        }
        [BasicAuthenticationAtrybut]
        public HttpResponseMessage Put(Koszykarz koszykarz)
        {
            List<string> errors = new List<string>();
            if (koszykarz.Haslo == null || koszykarz.Haslo == "")
                errors.Add("Brak hasła");
            if (koszykarz.Imie == null || koszykarz.Imie == "")
                errors.Add("Brak imienia");
            if (koszykarz.Login == null || koszykarz.Login == "")
                errors.Add("Brak loginu");
            if (koszykarz.Nazwisko == null || koszykarz.Nazwisko == "")
                errors.Add("Brak nazwiska");
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
                    db.Entry(koszykarz).State = System.Data.Entity.EntityState.Modified;
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
