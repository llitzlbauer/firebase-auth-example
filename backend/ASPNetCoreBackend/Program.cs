using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Lib.BusinessLogic.Implementations;
using Lib.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();


AddFirebaseAdmin();
builder.Services.AddControllers();
builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/fir-auth-example-a3add";
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/fir-auth-example-a3add",
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();



void AddFirebaseAdmin()
{
    var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "Util/fir-auth-example-a3add-firebase-adminsdk-w794j-aded73c601.json");
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(pathToKey)
    });
}
