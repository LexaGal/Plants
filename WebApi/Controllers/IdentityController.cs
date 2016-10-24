using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using AspNet.Identity.MySQL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApi.Controllers
{
    public class IdentityController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public IdentityController()
        {
        }

        public IdentityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        [HttpPost]
        [Route("api/identity/login")]
        public HttpResponseMessage Login(LoginViewModel model)
        {
            var applicationUser = UserManager.FindByEmailAsync(model.Email).Result;
            if (applicationUser != null)
            {
                var result = SignInManager.PasswordSignIn(applicationUser.UserName, model.Password, model.RememberMe,
                    true);
                switch (result)
                {
                    case SignInStatus.Success:
                        return new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            ReasonPhrase = "Successful login.",
                            Content = new StringContent(applicationUser.UserName, Encoding.UTF8, "application/json")
                        };
                    case SignInStatus.LockedOut:
                        return new HttpResponseMessage(HttpStatusCode.Forbidden) {ReasonPhrase = "Locked out."};
                    case SignInStatus.Failure:
                        return new HttpResponseMessage(HttpStatusCode.Forbidden)
                        {
                            ReasonPhrase = "User with this email and password does not exist."
                        };
                }
                return new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "Successful login."
                };
            }
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = "User with this email does not exist."
            };
        }

        [HttpPost]
        [Route("api/identity/register")]
        public HttpResponseMessage Register(RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "The password and confirmation password do not match."
                };
            }
            var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
            var result = UserManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                SignInManager.SignIn(user, false, false);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    ReasonPhrase = "Successful registration."
                };
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = result.Errors.First() };
        }
    }
}
