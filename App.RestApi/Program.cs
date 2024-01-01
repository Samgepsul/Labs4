using System.Reflection;

using MediatR;

using NLog.Web;

using RestApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Host.UseNLog();
//mediatr pattern
builder.Services.AddMediatR(f => f.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
//jwt auth
//builder.Services.AddJwt();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
