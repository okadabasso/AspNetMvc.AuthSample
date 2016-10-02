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
    public class AuthSignInManager : SignInManager<AuthUser, string>
    {
        public AuthSignInManager(UserManager<AuthUser, string> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static AuthSignInManager Create(IdentityFactoryOptions<AuthSignInManager> options, IOwinContext context)
        {
            return new AuthSignInManager(context.GetUserManager<AuthUserManager>(), context.Authentication);
        }
    }
}
