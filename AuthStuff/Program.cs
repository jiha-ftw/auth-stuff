using AuthStuff.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAzureAdAuth(builder.Configuration)
    .AddAuth0Auth(builder.Configuration)
    .SetupAuthentication()
    .SetupAuthorization()
    .AddMvc();

var app = builder.Build();

app.UseAuthentication()
    .UseRouting()
    .UseAuthorization()
    .SetupRoutes();

app.Run();
