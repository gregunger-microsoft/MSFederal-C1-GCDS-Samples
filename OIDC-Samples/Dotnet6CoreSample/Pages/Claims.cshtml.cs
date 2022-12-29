using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Dotnet6CoreSample.Pages
{
    public class ClaimsModel : PageModel
    {
        private IEnumerable<Claim>? myClaims;

        public IEnumerable<Claim>? MyClaims
        {
            get { return myClaims; }
            set { myClaims = value; }
        }


        public void OnGet()
        {
            //You can get the user and set the display name in the model class for the Page (or in the cshtml file if you refer to index.cshtml)
            var myUser = User.Identity as System.Security.Claims.ClaimsIdentity;

            myClaims = (myUser != null) ? myUser.Claims : new List<Claim>();

            ViewData["myDisplayName"] = myUser?.FindFirst("common_name")?.Value ?? myUser?.FindFirst("edipi")?.Value;
        }
    }
}
