using System.Reflection;
using ValidApi.Filters;
using ValidApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// Services
builder.Services.AddSingleton<IProfileParameterService, ProfileParameterService>();
builder.Services.AddHostedService<ProfileParameterUpdater>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<ProfileParameterSchemaFilter>();
});

var app = builder.Build();

var profileService = app.Services.GetRequiredService<IProfileParameterService>();
profileService.LoadParameters();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
