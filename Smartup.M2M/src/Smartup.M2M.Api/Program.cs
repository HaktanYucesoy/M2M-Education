using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Smartup.M2M.BuildingBlocks.Contracts.Pagination;
using Smartup.M2M.BuildingBlocks.Exceptions;
using Smartup.M2M.BuildingBlocks.Extensions;
using Smartup.M2M.Education.Infrastructure.Persistence.EfCore.Extensions;
using Smartup.M2M.Education.Infrastructure.Providers.Extensions;
using Smartup.M2M.Education.Module.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEduDbServices(builder.Configuration.GetConnectionString("M2MDb")!)
    .AddStudentCourseProvider()
    .AddM2MAuthentication(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // 1. Nesne örneklerinin var olduğundan emin ol
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        // 2. ClientId için Security Scheme tanımı eklendi
        document.Components.SecuritySchemes.Add("ClientId", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            Name = "X-Client-Id",
            In = ParameterLocation.Header
        });

        // 3. ClientSecret için Security Scheme tanımı eklendi
        document.Components.SecuritySchemes.Add("ClientSecret", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            Name = "X-Client-Secret",
            In = ParameterLocation.Header
        });

      
        document.Security =[
        new OpenApiSecurityRequirement{
          { new OpenApiSecuritySchemeReference("ClientId"), [] },
          { new OpenApiSecuritySchemeReference("ClientSecret"), [] }
        }
       ];
        document.SetReferenceHostDocument();

        return Task.CompletedTask;
    });
});

var app = builder.Build();

await app.Services.ApplyMigrationsAndSeedAsync();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapOpenApi();
app.UseSwaggerUI(c =>
{
    
    c.SwaggerEndpoint("/openapi/v1.json", "Smartup M2M API v1");
    c.RoutePrefix = string.Empty;
});
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

var educationGroup = app.MapGroup("/api/v1/education")
    .RequireAuthorization() 
    .WithTags("Edu M2M");

educationGroup.MapGet("/student-courses", async (
    [FromQuery] int? pageNumber,
    [FromQuery] int? pageSize,
    [FromQuery] string? criteria, // Artık ekranda sadece bu tekil ve temiz criteria görünecek
    [FromServices] IStudentCourseProvider provider,
    CancellationToken cancellationToken) =>
{
    PageRequest? request = null;

    if (pageNumber.HasValue && pageSize.HasValue)
    {
        request = new PageRequest
        {
            PageNumber = pageNumber.Value,
            PageSize = pageSize.Value
        };
    }

    var result = await provider.GetStudentCoursesAsync(request, criteria, cancellationToken);

    return result.ToHttpResult();
});

app.Run();
