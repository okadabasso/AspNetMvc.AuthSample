using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using AuthCore;
using AuthSample2.Models;

namespace AuthSample2.Controllers
{
    public class UsersController : Controller
    {
        private AuthDbContext db = new AuthDbContext();
        private AuthUserManager _userManager;
        private AuthSignInManager _signInManager;
        public UsersController()
        {
        }

        public UsersController(AuthUserManager userManager, AuthSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AuthSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AuthSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AuthUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AuthUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.AuthUsers.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthUser user = await db.AuthUsers.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserName,PasswordHash")] AuthUser user)
        {
            if (ModelState.IsValid)
            {
                var userForCreate = new AuthUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = user.UserName
                };
                var result = await UserManager.CreateAsync(userForCreate, user.PasswordHash);
                if (result.Succeeded)
                {
                    // 作成したユーザで即ログインする。
                    var signInUser = await UserManager.FindByNameAsync(userForCreate.UserName);
                    if (signInUser == null)
                    {
                        return View(user);
                    }
                    await SignInManager.SignInAsync(signInUser, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserName,Password,Memo")] AuthUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuthUser user = await db.AuthUsers.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AuthUser user = await db.AuthUsers.FindAsync(id);
            db.AuthUsers.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(AuthUser user, string returnUrl)
        {
            var userForLogin = await UserManager.FindAsync(user.UserName, user.PasswordHash);
            if (userForLogin == null)
            {
                return View(user);
            }

            await SignInManager.SignInAsync(userForLogin, false, false);

            return RedirectToLocal(returnUrl);
        }
        // POST: /Home/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Users");
        }
        [Authorize]
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index");
            }
            AddErrors(result);
            return View(model);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #region ヘルパー
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
    }
