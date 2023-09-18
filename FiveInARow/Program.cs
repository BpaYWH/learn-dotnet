using Microsoft.EntityFrameworkCore;
using FiveInARow.Context;

// for dependency injection & configuration
var builder = WebApplication.CreateBuilder(args);

{
    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //
    builder.Services.AddDbContext<FiveInARowDbContext>(options => 
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("GomokuDb"));
    });
}

var app = builder.Build();

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    // Authorization, Authentication, CORS, etc.

    app.Run();
}
