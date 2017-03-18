using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasketballTinder.Controllers
{
    public class BasicAuthenticationAtrybut : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                var authorization = actionContext.Request.Headers.Authorization;
                if (authorization == null) { actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized); return; }

                string authorizationParameter = authorization.Parameter;

                if (String.IsNullOrEmpty(authorizationParameter)) { actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized); return; }

                Tuple<string, string> companyIdAndSecret = ExtractUserNameAndSecret(authorizationParameter);
                Entities entities = new Entities();
                UserToken token = entities.Tokeny.Where(x => x.Login == companyIdAndSecret.Item1 && x.Token == companyIdAndSecret.Item2).FirstOrDefault();
                if (token == null)
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    return;
                }
                else
                {
                    if (token.ExpireDate < DateTime.Now)
                    {
                        actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                        entities.Entry(token).State = System.Data.Entity.EntityState.Deleted;
                        entities.SaveChanges();
                        return;
                    }
                    return;

                }

                
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized); return;
            }

        }

        private static Tuple<string, string> ExtractUserNameAndSecret(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string login = decodedCredentials.Substring(0, colonIndex);
            string token = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(login, token);
        }
    }
}