using Asp.Versioning.ApiExplorer;
using Cart.Infrastructure;
using Catalog.Infrastructure;
using Order.Infrastructure;
using Shopsy.API;
using User.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiServices();
builder.Services.AddUserModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddCartModule(builder.Configuration);
builder.Services.AddOrderModule(builder.Configuration);

var app = builder.Build();

// Apply Migrations & Seed
await app.Services.ApplyCatalogMigrationsAndSeed();
await app.Services.ApplyCartMigrationsAndSeed();
await app.Services.ApplyOrderMigrationsAndSeed();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();