	<add key="ClientId" value="localhost-44318" />
	<add key="Authority" value="https://federation.dev.cce.af.mil/oidc/localhost-44318" />
	<add key="RedirectUri" value="https://localhost:44318/" />
	<add key="BypassTlsDoDCertificateValidation" value="false" />


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



  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:17322",
      "sslPort": 44325
    }
  }


  "CallbackPath": "/signin-oidc"