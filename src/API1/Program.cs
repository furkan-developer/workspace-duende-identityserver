using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt => {

        // base-address of your identityserver
        opt.Authority = "https://localhost:5001";

        // Audience validation is disabled here because access to the api is modelled with ApiScopes only (scope-only model).
        // https://docs.duendesoftware.com/identityserver/v7/quickstarts/1_client_credentials/#add-jwt-bearer-authentication
        opt.TokenValidationParameters.ValidateAudience = false;


        // it's recommended to check the type header to avoid "JWT confusion" attacks
        opt.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/identity", (ClaimsPrincipal user) => user.Claims.Select(c => new {c.Type, c.Value}))
    .RequireAuthorization();

app.Run();
