using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Solution.DAL.Context;
using Solution.Dipatcher;
using Solution.Dipatcher.CommandDispatcher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce Solution", Version = "v1" });

});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("ECommerce"));
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
new DependecyInjection().RegisterComponents(builder.Services);

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
        s.RoutePrefix = string.Empty;
    });
}



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();