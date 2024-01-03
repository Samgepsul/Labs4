using System.Reflection;

using MediatR;

using NLog.Web;

using RestApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Services.AddMediatR(f => f.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();
