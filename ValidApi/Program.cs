using Microsoft.OpenApi.Models;
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

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Valid Api",
        Version = "v1",
        Description = @"API para gerenciamento de perfis e permissões.

        Códigos de retorno esperados:

        - 200 OK - Requisição bem-sucedida.
        - 201 Created - Perfil criado com sucesso.
        - 204 No Content - Operação realizada com sucesso, sem conteúdo no retorno.
        - 400 Bad Request - Erro nos dados enviados pelo cliente.
        - 404 Not Found - Recurso não encontrado.
        - 409 Conflict - Perfil já existe.
        - 500 Internal Server Error - Erro interno no servidor."
    });    
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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Valid Api");
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
