using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Lib.BusinessLogic.Implementations;
using Lib.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>();


AddFirebaseAdmin();
builder.Services.AddControllers();
builder.Services.AddSingleton(FirebaseAuth.DefaultInstance);
builder.Services.AddTransient<IUserService, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

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
