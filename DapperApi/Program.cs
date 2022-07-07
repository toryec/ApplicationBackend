
using DapperApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*var logging = builder.Configuration.GetSection(nameof(DapperApi.Logging)).Get<DapperApi.Logging>();

var logging2 = new DapperApi.Logging();
builder.Configuration.GetSection(nameof(DapperApi.Logging)).Bind(logging2);

///These three
builder.Services.AddOptions<DapperApi.Logging>()
    .BindConfiguration(nameof(DapperApi.Logging));

builder.Services.AddOptions<DapperApi.Logging>()
    .Bind(builder.Configuration.GetSection(nameof(DapperApi.Logging)))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .Configure<DapperApi.Logging>(builder.Configuration.GetSection(nameof(DapperApi.Logging)));
///These three*/


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
