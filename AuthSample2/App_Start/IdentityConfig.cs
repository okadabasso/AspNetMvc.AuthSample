using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AuthCore;
using AuthSample2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;


namespace AuthSample2
{
    public class IdentityConfig
    {
    }
    public class ApplicationUserManager : UserManager<AuthCore.AuthUser>
    {
        public ApplicationUserManager(IUserStore<AuthCore.AuthUser> store) : base(store)
        {
            
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new AuthCore.UserStore(context.Get<ApplicationDbContext>()));
            // ユーザー ロックアウトの既定値を設定します。
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<AuthCore.AuthUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class ApplicationSignInManager : SignInManager<AuthCore.AuthUser, string>
    {
        public ApplicationSignInManager(UserManager<AuthCore.AuthUser, string> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class UserValidator : Microsoft.AspNet.Identity.UserValidator<AuthCore.AuthUser, string>
    {
        public UserValidator(UserManager<AuthUser, string> manager) : base(manager)
        {
        }

        public override Task<IdentityResult> ValidateAsync(AuthUser item)
        {
            
            return base.ValidateAsync(item);
        }
    }

}