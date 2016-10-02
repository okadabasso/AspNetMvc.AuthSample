using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AuthCore
{
    public class AuthUserManager : UserManager<AuthUser>
    {
        public AuthUserManager(IUserStore<AuthUser> store) : base(store)
        {
        }

        public static AuthUserManager Create(IdentityFactoryOptions<AuthUserManager> options, IOwinContext context)
        {
            return new AuthUserManager(new UserStore(context.Get<AuthDbContext>()));
        }

    }
}
