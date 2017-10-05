using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TCFP.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize,HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet,AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Login(Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new Data.TCFPEntities())
                {
                    var result = context.sp_Login(model.Email, model.Password).SingleOrDefault();

                    if (result != 1)
                    {
                        ModelState.AddModelError("E0010", Resources.Message.E0010);
                        return View();
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return View();
            }
        }
    }
}