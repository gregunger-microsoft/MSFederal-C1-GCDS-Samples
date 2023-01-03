using System;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Owin;
using Microsoft.Owin.Security.Notifications;

[assembly: OwinStartup(typeof(DotnetFramework472Sample.Startup))]

namespace DotnetFramework472Sample
{
    public partial class Startup
    {
        //private static X509Certificate2 verifyCert = null;

        readonly string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
        // readonly string clientSecret = System.Configuration.ConfigurationManager.AppSettings["ClientSecret"];
        readonly string redirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
        readonly string authority = System.Configuration.ConfigurationManager.AppSettings["Authority"];
        readonly bool bypassTlsDoDCertificateValidation = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["BypassTlsDoDCertificateValidation"]);
        readonly static string gcdsAuthCertThumbprint = System.Configuration.ConfigurationManager.AppSettings["GcdsAuthCertThumbprint"];


        public void Configuration(IAppBuilder app)
        {
            if (bypassTlsDoDCertificateValidation)
            {
                // Trust all certificates
                ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            }
            else
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateCertificate);
            }

            //app.UseCors(CorsOptions.AllowAll);
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(
            new OpenIdConnectAuthenticationOptions
            {
                // Sets the ClientId, authority, RedirectUri as obtained from web.config
                ClientId = clientId,
                //ClientSecret = clientSecret,
                Authority = authority,
                RedirectUri = redirectUri,
                // PostLogoutRedirectUri is the page that users will be redirected to after sign-out. In this case, it is using the home page
                PostLogoutRedirectUri = redirectUri,
                ResponseType = OpenIdConnectResponseType.Code,
                // OpenIdConnectAuthenticationNotifications configures OWIN to send notification of failed authentications to OnAuthenticationFailed method
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = OnAuthenticationFailed
                },
                RedeemCode = true,
                UsePkce = true
            }
        );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate">This will be the certificate that the IdP sends to the client app</param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            var hashString = certificate.GetCertHashString(); // this is the cert from the IdP
            if (hashString != null)
            {
                var certHashString = hashString.ToLower();

                //grab cert from app settings
                bool result = certHashString.Equals(gcdsAuthCertThumbprint, StringComparison.OrdinalIgnoreCase); // DoD SW CA-60 crt file
                return result;
            }
            return false;
        }

        /// <summary>
        /// Handle failed authentication requests by redirecting the user to the home page with an error in the query string
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/?errormessage=" + context.Exception.Message);
            return Task.FromResult(0);
        }
    }
}
