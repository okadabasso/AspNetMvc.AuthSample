using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AuthCore;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace AuthSample2
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(AuthDbContext.Create);
            app.CreatePerOwinContext<AuthUserManager>(AuthUserManager.Create);
            app.CreatePerOwinContext<AuthSignInManager>(AuthSignInManager.Create);


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Users/Login")
            });
        }
    }
}