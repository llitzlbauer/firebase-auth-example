using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Lib.BusinessLogic.Implementations;
using Lib.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //TODO: eure project id einfügen
        options.Authority = "https://securetoken.google.com/fir-auth-example-8dbdb";
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = "https://securetoken.google.com/fir-auth-example-8dbdb",
            ValidAudience = "fir-auth-example-8dbdb",
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

AddFirebaseAdmin();
builder.Services.AddControllers();
builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);
builder.Services.AddTransient<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                        .AllowCredentials()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
});
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

app.UseCors("AllowAllOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseHttpLogging();


void AddFirebaseAdmin()
{
    //TODO: Firebase-Konsole -> Projekteinstellungen -> Dienstkonten -> Firebase Admin SDK -> Neuen privaten Schlüssel generieren
    //File dann unter util speichern
    var pathToKey = Path.Combine(Directory.GetCurrentDirectory(), "Util/fir-auth-example-8dbdb-firebase-adminsdk-w46s9-5f4d2b36bd.json");
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.FromFile(pathToKey)
    });
}