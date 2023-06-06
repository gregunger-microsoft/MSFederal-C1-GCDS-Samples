using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true; //Add this line

// Provision configuration subsystem
IConfiguration _Configuration = builder.Configuration;
bool _MyCustomBypassCertificateValidation = Convert.ToBoolean(_Configuration["MyCustomBypassCertificateValidation"]);

// Create certificate handler 
HttpClientHandler gregsCustomerCertificateValidationHandler = new HttpClientHandler
{
    // bypass certificate validation
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    string authority = _Configuration["OIDCConfig:Authority"];
    string clientId = _Configuration["OIDCConfig:ClientId"];
    string responseType = _Configuration["OIDCConfig:ResponseType"];
    options.Authority = authority;
    options.ClientId = clientId;
    options.ResponseType = responseType;
    options.UsePkce = true;

    // Do not set this
    //string callbackPath = _Configuration["OIDCConfig:CallbackPath"];
    //options.CallbackPath = callbackPath;

    // Check appsettings.json file to see if we should bypass the certificate validation
    if (_MyCustomBypassCertificateValidation)
    {
        // assign my custom handler
        options.BackchannelHttpHandler = gregsCustomerCertificateValidationHandler;
    }
});

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
