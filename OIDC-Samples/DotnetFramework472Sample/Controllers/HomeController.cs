using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotnetFramework472Sample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SetDisplayName();

            return View();
        }

        public ActionResult Claims()
        {
            SetDisplayName();

            ViewBag.Message = "Use this page to view all of the claims returned by the Authentication Provider.";

            return View();
        }

        private void SetDisplayName()
        {
            var displayName = "Nobody";

            if (System.Security.Claims.ClaimsPrincipal.Current.HasClaim(x => x.Type == "common_name"))
            {
                displayName = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("common_name").Value;
            }
            else if (System.Security.Claims.ClaimsPrincipal.Current.HasClaim(x => x.Type == "edipi"))
            {
                displayName = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("edipi").Value;
            }
            else
            {
                displayName = "No common_name or edipi claims found";
            }

            ViewBag.DisplayName = displayName;

        }
    }
}