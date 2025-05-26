
using Spendnt.API;
using Microsoft.EntityFrameworkCore;
using Spendnt.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Agrega los controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Spend'nt API",
        Version = "v1"
    });
});

// Inyecta el DataContext con cadena de conexión
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Inyecta el servicio SeedDb
builder.Services.AddTransient<SeedDB>();

var app = builder.Build();

//Middleware
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());


// Ejecuta la semilla de datos al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seeder = services.GetRequiredService<SeedDB>();
    await seeder.SeedAsync();
}

// Configuración para entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spend't API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
