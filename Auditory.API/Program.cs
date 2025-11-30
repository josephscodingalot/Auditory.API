using System.Text;
using Auditory.API.Middleware;
using Auditory.Application.Mappings;
using Auditory.Domain.Interfaces;
using Auditory.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Auditory.Application.Behaviors;
using Auditory.Application.Handlers;
using Auditory.Infrastructure.Auth;
using Auditory.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

var mongoSettings = builder.Configuration.GetSection("MongoDbSettings");

builder.Services.AddSingleton(new Auditory.Infrastructure.Persistence.MongoDbContext(
    mongoSettings.GetValue<string>("ConnectionString") ?? throw new InvalidOperationException("MongoDB connection string is not configured."),
    mongoSettings.GetValue<string>("DatabaseName") ?? throw new InvalidOperationException("MongoDB database name is not configured.")
));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


builder.Services.AddScoped<IStreamRepository, StreamRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();



//Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Auditory.Application.Queries.GetStreamsByUserQuery>());
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetStreamsByUserHandler>()); 

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

builder.Services.AddAutoMapper(typeof(StreamProfile));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.WebHost.UseUrls("http://localhost:8001");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000") // your React app URL
            .AllowAnyHeader()
            .AllowAnyMethod());
});


var app = builder.Build();

// Enable middleware to serve generated Swagger as a JSON endpoint and the Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Auditory API v1");
        c.RoutePrefix = "swagger";
    });
}


app.UseCors("AllowReactApp"); 
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();