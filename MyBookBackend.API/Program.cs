using Asp.Versioning;
using FluentValidation;
using MyBookBackend.API.Configurations;
using MyBookBackend.API.Extensions;
using MyBookBackend.API.Middlewares;
using MyBookBackend.Common.Interfaces;
using MyBookBackend.Common.Service;
using MyBookBackend.Common.Validators;
using MyBookBackend.Common.Validators.Auth;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging

builder.Host.AddSerilogConfiguration();

#endregion

#region Framework Services

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();

#endregion

#region API Versioning

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";

    options.SubstituteApiVersionInUrl = true;
});

#endregion

#region Swagger

builder.Services.AddSwaggerConfiguration();

#endregion

#region Database

builder.Services.AddApplicationDatabase(builder.Configuration);

#endregion

#region Authentication

builder.Services.AddJwtAuthentication(builder.Configuration);

#endregion

#region CORS

builder.Services.AddCorsConfiguration();

#endregion

#region Dependency Injection
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddRepositories();

builder.Services.AddApplicationServices();

#endregion

var app = builder.Build();

#region Middleware Pipeline

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();


    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        var provider =
            app.Services.GetRequiredService<
                Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();