using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddControllers();

//Add swagger generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auditory API",
        Version = "v1",
        Description = "API for managing Spotify listening history and personal charts",
        Contact = new OpenApiContact
        {
            Name = "Joseph",
            Url = new Uri("https://github.com/josephscodingalot")
        }
    });
});

var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint and the Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auditory API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();