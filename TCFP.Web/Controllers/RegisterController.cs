using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TCFP.Data;
using System.Configuration;
using Newtonsoft.Json;
using System.Transactions;
using System.Data.Entity.Core.EntityClient;
using System.Net.Mail;

namespace TCFP.Web.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private const string TokenIDKey = "TokenID";

        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Models.RegisterAccount model)
        {
            if (ModelState.IsValid)
            {
                // Check the result from Google reCAPTCHA
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", ConfigurationManager.AppSettings["reCAPTCHASecretKey"]),
                    new KeyValuePair<string, string>("response", Request["g-recaptcha-response"])
                });

                HttpResponseMessage resp;
                using (var client = new HttpClient())
                {
                    resp = await client.PostAsync(
                        ConfigurationManager.AppSettings["reCAPTCHASiteVerifyURL"],
                         formContent);
                }

                var respJSON = JsonConvert.DeserializeObject<JSONResponse>(await resp.Content.ReadAsStringAsync());
                if (respJSON.success != true)
                {
                    ModelState.AddModelError("E0002", Resources.Message.E0002);
                    return View(model);
                }
                else
                {
                    string tokenID;
                    //var context = new TCFP.Data.TCFPEntities();
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        using (var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["TCFPEntities"].ConnectionString))
                        {
                            await conn.OpenAsync();

                            using (var context = new TCFPEntities(conn,false))
                            {
                                tokenID = Guid.NewGuid().ToString("N");
                                int r = context.sp_RegisterNewClient(model.Email, model.Name, tokenID);
                                if (r == 0)
                                {
                                    ModelState.AddModelError("E0011", Resources.Message.E0011);
                                    return View();
                                }
                                else
                                    ViewBag.Message = Resources.Message.I0001;
                            }
                        }

                        // Send Email
                        using (MailMessage msg = new MailMessage(new MailAddress("no_reply@tcfp.com"), new MailAddress(model.Email)))
                        {
                            msg.Subject = Resources.EmailContent.RegisterSubject;
                            msg.IsBodyHtml = false;
                            msg.Body = string.Format(Resources.EmailContent.RegisterContent, model.Name, ConfigurationManager.AppSettings["BaseURL"] + "?t=" + tokenID);

                            using (SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"], int.Parse(ConfigurationManager.AppSettings["SMTPPort"])))
                            {
                                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"], ConfigurationManager.AppSettings["SMTPPwd"]);
                                await smtp.SendMailAsync(msg);
                            }
                        }

                        scope.Complete();
                    }

                    return View();
                }


            }
            else
            {
                return View(model);
            }
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

        class JSONRequest
        {
            public string secret;
            public string response;
        }
        
        class JSONResponse
        {
            public bool success;
            public DateTime challenge_ts;
            public string hostname;
            [JsonProperty(PropertyName = "error-codes")]
            public string[] error_codes;
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            string tokenID = Request.QueryString["t"];
            if (string.IsNullOrEmpty(tokenID))
            {
                ModelState.AddModelError("E0008", Resources.Message.E0008);
                return View();
            }
            else
            {
                using (var context = new Data.TCFPEntities())
                {
                    var tokens = context.sp_GetUserToken(tokenID).ToList();
                    if (tokens.Count != 1)
                    {
                        ModelState.AddModelError("E0008", Resources.Message.E0008);
                        return View();
                    }
                    else
                    {
                        var token = tokens.SingleOrDefault();
                        if (token.ExpiredOn<=DateTime.Now || token.UsedOn.HasValue)
                        {
                            ModelState.AddModelError("E0009", Resources.Message.E0009);
                            return View();
                        }
                        else
                        {
                            TempData[TokenIDKey] = token.TokenID;
                            return View();
                        }
                    }
                }
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(Models.NewPassword model)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var conn = new EntityConnection(ConfigurationManager.ConnectionStrings["TCFPEntities"].ConnectionString))
                    {
                        await conn.OpenAsync();

                        using (var context = new TCFPEntities(conn, false))
                        {
                            int affectedRecord = context.sp_ResetPassword(TempData[TokenIDKey].ToString(), model.Password);

                            if (affectedRecord == 0)
                            {
                                ModelState.AddModelError("E0009", Resources.Message.E0009);
                            }
                            else
                            {
                                ViewBag.Message = Resources.Message.I0002;
                            }
                        }
                    }

                    scope.Complete();
                }

                return View();
            }
            else
                return View();
        }

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgetPassword(Models.ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // Check the result from Google reCAPTCHA
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", ConfigurationManager.AppSettings["reCAPTCHASecretKey"]),
                    new KeyValuePair<string, string>("response", Request["g-recaptcha-response"])
                });

                HttpResponseMessage resp;
                using (var client = new HttpClient())
                {
                    resp = await client.PostAsync(
                        ConfigurationManager.AppSettings["reCAPTCHASiteVerifyURL"],
                         formContent);
                }

                var respJSON = JsonConvert.DeserializeObject<JSONResponse>(await resp.Content.ReadAsStringAsync());
                if (respJSON.success != true)
                {
                    ModelState.AddModelError("E0002", Resources.Message.E0002);
                    return View(model);
                }
                else
                {
                    string tokenID;
                    using (var context = new TCFPEntities())
                    {
                        tokenID = Guid.NewGuid().ToString("N");
                        var r = context.sp_ForgetPassword(model.Email, tokenID).SingleOrDefault();

                        if (r!=1)
                        {
                            ModelState.AddModelError("E0012", Resources.Message.E0012);
                            return View();
                        }
                        else
                            ViewBag.Message = Resources.Message.I0003;
                    }


                    // Send Email
                    using (MailMessage msg = new MailMessage(new MailAddress("no_reply@tcfp.com"), new MailAddress(model.Email)))
                    {
                        msg.Subject = Resources.EmailContent.RegisterSubject;
                        msg.IsBodyHtml = false;
                        msg.Body = string.Format(Resources.EmailContent.RegisterContent, "Customer", ConfigurationManager.AppSettings["BaseURL"] + "?t=" + tokenID);

                        using (SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"], int.Parse(ConfigurationManager.AppSettings["SMTPPort"])))
                        {
                            smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"], ConfigurationManager.AppSettings["SMTPPwd"]);
                            await smtp.SendMailAsync(msg);
                        }
                    }

                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}